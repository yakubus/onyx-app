import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:onyx_app/config.dart';
import 'user.dart';

class UserService {
  final String apiUrl = '${Config.API_URL}/user';

  Future<UserServiceModel> loginUser(String email, String password) async {
    final response = await http.post(
      Uri.parse("$apiUrl/login"),
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({"email": email, "password": password}),
    );

    if (response.statusCode == 200) {
      return UserServiceModel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('Failed to load user');
    }
  }

  Future<UserServiceModel> registerUser(
      String email, String password, String currency, String username) async {
    final response = await http.post(
      Uri.parse("$apiUrl/register"),
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({
        "email": email,
        "password": password,
        "currency": currency,
        "username": username,
      }),
    );

    if (response.statusCode == 200) {
      return UserServiceModel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('Failed to register user');
    }
  }
}
