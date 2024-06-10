import 'dart:async';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/models/error.dart';
import 'package:onyx_app/services/user/user_repo.dart';

class UserServiceModel {
  final bool isSuccess;
  final bool isFailure;
  final ErrorOnyx error;
  final UserData value;

  UserServiceModel(
      {required this.isSuccess,
      required this.isFailure,
      required this.error,
      required this.value});

  factory UserServiceModel.fromJson(Map<String, dynamic> json) {
    return UserServiceModel(
      isSuccess: json['isSuccess'],
      isFailure: json['isFailure'],
      error: ErrorOnyx.fromJson(json['error']),
      value: UserData.fromJson(json['value']),
    );
  }
}

class UserData {
  final String id;
  final String username;
  final String email;
  final String currency;
  final String accessToken;
  final List<String> budgetIds;

  UserData(
      {required this.id,
      required this.username,
      required this.email,
      required this.currency,
      required this.accessToken,
      required this.budgetIds});

  factory UserData.fromJson(Map<String, dynamic> json) {
    return UserData(
      id: json['id'],
      username: json['username'],
      email: json['email'],
      currency: json['currency'],
      accessToken: json['accessToken'],
      budgetIds: List<String>.from(json['budgetIds'].map((x) => x)),
    );
  }
}

class UserNotifier extends AsyncNotifier<UserServiceModel> {
  UserNotifier(this.token);

  String token;

  @override
  FutureOr<UserServiceModel> build() {
    if (token.isEmpty) {
      return UserServiceModel(
          isSuccess: false,
          isFailure: true,
          error: const ErrorOnyx(message: 'Token is empty', code: "401"),
          value: UserData(
              accessToken: '',
              budgetIds: [],
              currency: '',
              email: '',
              id: '',
              username: ''));
    }
    return ref.read(userServiceProvider).getUserData(token);
  }

  Future<void> refresh() async {
    state = await AsyncValue.guard(
        () => ref.watch(userServiceProvider).getUserData(token));
  }

  Future<UserServiceModel> login(String email, String password) async {
    UserServiceModel user =
        await ref.read(userServiceProvider).loginUser(email, password);
    token = user.value.accessToken;
    refresh();
    return user;
  }

  Future<void> register(
      String email, String password, String currency, String username) async {
    ref
        .read(userServiceProvider)
        .registerUser(email, password, currency, username);
    refresh();
  }
}
