import 'dart:convert';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;

class ApiService {
  static const String baseUrl = 'http://10.0.2.2:5254/api/users';

  final FlutterSecureStorage storage = const FlutterSecureStorage();
  final http.Client client = http.Client();

  static const String keyAccessToken = 'access_token';
  static const String keyRefreshToken = 'refresh_token';

  Future<http.Response> post(String endpoint, Map<String, dynamic> body) async {
    final url = Uri.parse('$baseUrl$endpoint');
    final token = await storage.read(key: keyAccessToken);

    var response = await client.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        if (token != null) 'Authorization': 'Bearer $token',
      },
      body: jsonEncode(body),
    );

    if (response.statusCode == 401) {
      final refreshed = await handleTokenRefresh();

      if (refreshed) {
        final newToken = await storage.read(key: keyAccessToken);

        response = await client.post(
          url,
          headers: {
            'Content-Type': 'application/json',
            if (newToken != null) 'Authorization': 'Bearer $newToken',
          },
          body: jsonEncode(body),
        );
      }
    }

    return response;
  }

  Future<http.Response> get(String endpoint) async {
    final url = Uri.parse('$baseUrl$endpoint');
    final token = await storage.read(key: keyAccessToken);

    var response = await client.get(
      url,
      headers: {
        'Content-Type': 'application/json',
        if (token != null) 'Authorization': 'Bearer $token',
      },
    );

    if (response.statusCode == 401) {
      final refreshed = await handleTokenRefresh();

      if (refreshed) {
        final newToken = await storage.read(key: keyAccessToken);

        response = await client.get(
          url,
          headers: {
            'Content-Type': 'application/json',
            if (newToken != null) 'Authorization': 'Bearer $newToken',
          },
        );
      }
    }

    return response;
  }

  Future<Map<String, dynamic>> processAuthResponse(
    http.Response response,
  ) async {
    final data = jsonDecode(response.body);

    if (response.statusCode == 200 || response.statusCode == 201) {
      final accessToken = data['accessToken'] ?? data['token'];
      final refreshToken = data['refreshToken'];

      if (accessToken != null && refreshToken != null) {
        await storage.write(key: keyAccessToken, value: accessToken);
        await storage.write(key: keyRefreshToken, value: refreshToken);
      }

      return {'success': true, 'data': data};
    }

    return {
      'success': false,
      'message': data['message'] ?? 'An error occurred',
    };
  }

  Future<bool> handleTokenRefresh() async {
    final expiredAccess = await storage.read(key: keyAccessToken);
    final refreshToken = await storage.read(key: keyRefreshToken);

    if (expiredAccess == null || refreshToken == null) {
      await logout();
      return false;
    }

    try {
      final response = await client.post(
        Uri.parse('$baseUrl/refresh'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'accessToken': expiredAccess,
          'refreshToken': refreshToken,
        }),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);

        final newAccess = data['accessToken'] ?? data['token'];
        final newRefresh = data['refreshToken'];

        if (newAccess != null && newRefresh != null) {
          await storage.write(key: keyAccessToken, value: newAccess);
          await storage.write(key: keyRefreshToken, value: newRefresh);

          return true;
        }
      }
    } catch (_) {}

    await logout();
    return false;
  }

  Future<void> logout() async {
    await storage.delete(key: keyAccessToken);
    await storage.delete(key: keyRefreshToken);
  }

  Future<String?> getStoredToken() async {
    return storage.read(key: keyAccessToken);
  }
}
