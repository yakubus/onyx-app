import 'dart:convert';
import 'dart:developer';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:http/http.dart' as http;
import 'package:onyx_app/config.dart';
import 'user.dart';

final userServiceProvider = Provider((ref) => UserService());

class UserService {
  final String apiUrl = '${Config.API_URL}/user';

  Future<UserServiceModel> getUserData(String token) async {
    final response = await http.get(
      Uri.parse(apiUrl),
      headers: {
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      final Map<String, dynamic> data = json.decode(response.body);
      return UserServiceModel.fromJson(data);
    } else {
      throw Exception('Failed to load user data');
    }
  }

  Future<UserServiceModel> loginUser(String email, String password) async {
    final response = await http
        .post(
      Uri.parse("$apiUrl/login"),
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({"email": email, "password": password}),
    )
        .timeout(const Duration(seconds: 15), onTimeout: () {
      throw Exception('Connection timeout');
    });
    if (response.statusCode == 200) {
      return UserServiceModel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('${response.statusCode}:Failed to login user');
    }
  }

  Future<UserServiceModel> registerUser(
      String email, String password, String currency, String username) async {
    final response = await http
        .post(
      Uri.parse("$apiUrl/register"),
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({
        "email": email,
        "password": password,
        "currency": currency,
        "username": username,
      }),
    )
        .timeout(const Duration(seconds: 15), onTimeout: () {
      throw Exception('Connection timeout');
    });

    if (response.statusCode == 200) {
      return UserServiceModel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('${response.statusCode}:Failed to register user');
    }
  }
}
