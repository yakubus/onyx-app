import 'dart:async';
import 'dart:developer';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/models/error.dart';
import 'package:onyx_app/services/budget/budget.dart';
import 'package:onyx_app/services/saving_read_data_locally.dart';
import 'package:onyx_app/services/user/user_repo.dart';

final userDataServiceProvider =
    StateProvider<UserServiceModel>((ref) => UserServiceModel());

class UserServiceModel {
  final bool? isSuccess;
  final bool? isFailure;
  final ErrorOnyx? error;
  final UserData? value;

  UserServiceModel({this.isSuccess, this.isFailure, this.error, this.value});

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
  @override
  FutureOr<UserServiceModel> build() async {
    String token = ref.watch(userToken.notifier).state;
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
    ref.watch(userDataServiceProvider.notifier).state =
        await ref.read(userServiceProvider).getUserData(token);
    return ref.read(userServiceProvider).getUserData(token);
  }

  Future<void> refresh() async {
    String token = ref.watch(userToken.notifier).state;
    state = await AsyncValue.guard(
        () => ref.watch(userServiceProvider).getUserData(token));
  }

  Future<UserData> login(String email, String password) async {
    try {
      ref.read(userDataServiceProvider.notifier).state =
          await ref.read(userServiceProvider).loginUser(email, password);

      ref.read(userToken.notifier).state =
          ref.read(userDataServiceProvider.notifier).state.value?.accessToken ??
              '';
      ref.read(isLogged.notifier).state =
          ref.read(userDataServiceProvider.notifier).state.isSuccess ?? false;

      refresh();

      log("userToken: ${ref.read(userToken.notifier).state}");
      return ref.read(userDataServiceProvider.notifier).state.value!;
    } catch (e) {
      log("Login error: $e");
      rethrow;
    }
  }

  Future<UserData> register(
      String email, String password, String currency, String username) async {
    try {
      // Rejestracja użytkownika
      ref.read(userDataServiceProvider.notifier).state = await ref
          .read(userServiceProvider)
          .registerUser(email, password, currency, username);

      // Ustawienie tokenu i statusu logowania
      ref.read(userToken.notifier).state =
          ref.read(userDataServiceProvider.notifier).state.value?.accessToken ??
              '';
      ref.read(isLogged.notifier).state =
          ref.read(userDataServiceProvider.notifier).state.isSuccess ?? false;

      // Odświeżenie stanu
      refresh();

      log("userToken: ${ref.read(userToken.notifier).state}");
      log("isLogged: ${ref.read(isLogged.notifier).state}");
      return ref.read(userDataServiceProvider.notifier).state.value!;
    } catch (e) {
      log("Register error: $e");
      rethrow;
    }
  }

  void userLogout() {
    ref.read(userToken.notifier).state = '';
    ref.read(isLogged.notifier).state = false;
    ref.read(userDataServiceProvider.notifier).state = UserServiceModel();
    ref.read(budgetServiceDataProvider.notifier).state = AsyncValue.data(
      BudgetServiceModel(
        value: [],
        isSuccess: false,
        isFailure: true,
        error: const ErrorOnyx(message: 'Token is empty', code: "401"),
      ),
    );

    clearAppPreferences();
  }
}
