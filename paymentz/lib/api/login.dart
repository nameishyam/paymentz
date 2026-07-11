import 'dart:convert';
import '../services/api.dart';

class LoginService {
  final ApiService _api = ApiService();

  Future<Map<String, dynamic>> login(String email, String password) async {
    try {
      final response = await _api.client.post(
        Uri.parse('${ApiService.baseUrl}/login'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'email': email, 'password': password}),
      );

      return await _api.processAuthResponse(response);
    } catch (e) {
      return {'success': false, 'message': 'Connection failed: $e'};
    }
  }
}
