import 'dart:convert';

import '../services/api.dart';

class SignupService {
  final ApiService _api = ApiService();

  Future<Map<String, dynamic>> signup({
    required String email,
    required String password,
    required String firstName,
    required String lastName,
  }) async {
    try {
      final response = await _api.client.post(
        Uri.parse('${ApiService.baseUrl}/signup'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'firstName': firstName,
          'lastName': lastName,
          'email': email,
          'password': password,
        }),
      );

      return await _api.processAuthResponse(response);
    } catch (e) {
      return {'success': false, 'message': 'Connection failed: $e'};
    }
  }
}
