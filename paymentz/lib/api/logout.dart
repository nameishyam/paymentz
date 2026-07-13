import '../services/api.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class LogoutService {
  final ApiService _api = ApiService();
  final FlutterSecureStorage storage = const FlutterSecureStorage();
  static const String keyAccessToken = 'access_token';

  Future logout() async {
    try {
      final token = await storage.read(key: keyAccessToken);
      await _api.client.post(
        Uri.parse('${ApiService.baseUrl}/logout'),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
      );
    } catch (e) {
      return {'success': false, 'message': 'Connection failed : $e'};
    }
  }
}
