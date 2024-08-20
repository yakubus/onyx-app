// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:async';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/log/logger.dart';

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
      error: json['error'] != null ? ErrorOnyx.fromJson(json['error']) : null,
      value: json['value'] != null ? UserData.fromJson(json['value']) : null,
    );
  }
}

class UserData {
  final String? id;
  final String? username;
  final String? email;
  final String? currency;
  final String? accessToken;
  final String? longLivedToken;
  final List<String>? budgetIds;

  UserData({
    this.id,
    this.username,
    this.email,
    this.currency,
    this.accessToken,
    this.longLivedToken,
    this.budgetIds,
  });

  factory UserData.fromJson(Map<String, dynamic> json) {
    return UserData(
      id: json['id'] as String?,
      username: json['username'] as String?,
      email: json['email'] as String?,
      currency: json['currency'] as String?,
      accessToken: json['accessToken'] as String?,
      longLivedToken: json['longLivedToken'] as String?,
      budgetIds: json['budgetIds'] != null
          ? List<String>.from(json['budgetIds'])
          : null,
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
      // Wywołanie loginUser w celu uzyskania danych użytkownika
      final userDataModel =
          await ref.read(userServiceProvider).loginUser(email, password);

      // Sprawdzenie, czy zwrócony obiekt UserData nie jest null
      if (userDataModel.value == null) {
        throw Exception('Login failed: User data is null');
      }

      // Zapisanie UserData w stanie globalnym
      ref.read(userDataServiceProvider.notifier).state = userDataModel;

      // Aktualizacja tokenów i statusu logowania
      ref.read(userToken.notifier).state =
          userDataModel.value?.accessToken ?? '';

      ref.read(longLivedToken.notifier).state =
          userDataModel.value?.longLivedToken ?? '';

      await saveLongLivedToken(
          ref.read(longLivedToken.notifier).state.toString());
      ref.read(isLogged.notifier).state = userDataModel.isSuccess ?? false;

      // Odświeżenie danych użytkownika, jeśli logowanie się powiodło
      if (ref.read(isLogged.notifier).state) {
        await refresh();
      }

      logger("UserNotifier -> login: User is loged in");

      return userDataModel.value!;
    } catch (e) {
      logger("UserNotifier -> login: error {$e}");
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

      logger("UserNotifier -> register: ");
      return ref.read(userDataServiceProvider.notifier).state.value!;
    } catch (e) {
      logger("UserNotifier -> register: error {$e}");
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
    logger("UserNotifier -> userLogout: user is loged out");
  }

  void keepAlive() {
    if (ref.read(longLivedToken.notifier).state == null) return;
    ref
        .read(userServiceProvider)
        .keepALive(ref.read(longLivedToken.notifier).state ?? '');

    ref.read(userToken.notifier).state =
        ref.read(userDataServiceProvider.notifier).state.value?.accessToken ??
            '';
    ref.read(isLogged.notifier).state =
        ref.read(userDataServiceProvider.notifier).state.isSuccess ?? false;

    refresh();
    logger("UserNotifier -> keepAlive: try refresh user token");
  }
}
