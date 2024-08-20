import 'dart:convert';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:http/http.dart' as http;
import 'package:onyx_app/config.dart';
import 'package:onyx_app/log/logger.dart';
import 'user.dart';

final userServiceProvider = Provider((ref) => UserService());

class UserService {
  final String apiUrl = '${Config.API_URL_ID}/user';
  final String apiAuthUrl = '${Config.API_URL_ID}/auth';

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
      Uri.parse("$apiAuthUrl/login"),
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({"email": email, "password": password}),
    )
        .timeout(const Duration(seconds: 15), onTimeout: () {
      throw Exception('Connection timeout');
    });
    if (response.statusCode == 200) {
      logger("UserService -> loginUser service response: ${response.body}");
      final Map<String, dynamic> responseData = jsonDecode(response.body);
      return UserServiceModel.fromJson(responseData);
    } else {
      logger(
          "UserService -> loginUser exception service response: ${response.body}");
      throw Exception('${response.statusCode}: Failed to login user');
    }
  }

  Future<UserServiceModel> registerUser(
      String email, String password, String currency, String username) async {
    final response = await http
        .post(
      Uri.parse("$apiAuthUrl/register"),
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
      logger("UserService-> registerUser service response: ${response.body}");
      return UserServiceModel.fromJson(jsonDecode(response.body));
    } else {
      logger(
          "UserService -> registerUser exeption service response: ${response.body}");
      throw Exception('${response.statusCode}:Failed to register user');
    }
  }

  Future<UserServiceModel> keepALive(String longLivedToken) async {
    final endpoint = Uri.parse("$apiAuthUrl/refresh");
    final headers = {"Content-Type": "application/json"};
    final body = jsonEncode({"longLivedToken": longLivedToken});

    logger("request endpoint: $endpoint, headers: $headers, body: $body");
    final response = await http
        .post(
      endpoint,
      headers: headers,
      body: body,
    )
        .timeout(const Duration(seconds: 15), onTimeout: () {
      throw Exception('Connection timeout');
    });

    if (response.statusCode == 200) {
      logger("response: ${response.body}");
      return UserServiceModel.fromJson(jsonDecode(response.body));
    } else {
      logger("response: ${response.body}");
      throw Exception('${response.statusCode}:Failed to login user');
    }
  }
}
