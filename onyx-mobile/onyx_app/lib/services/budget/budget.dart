// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter/foundation.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/main.dart';

import 'package:onyx_app/models/category.dart';
import 'package:onyx_app/models/counterparty.dart';
import 'package:onyx_app/models/error.dart';
import 'package:onyx_app/services/budget/budget_repo.dart';
import 'package:onyx_app/views/accounts/accounts.dart';

class BudgetServiceModel {
  List<Budget> value;
  bool isSuccess;
  bool isFailure;
  ErrorOnyx error;

  BudgetServiceModel({
    required this.value,
    required this.isSuccess,
    required this.isFailure,
    required this.error,
  });

  BudgetServiceModel copyWith({
    List<Budget>? value,
    bool? isSuccess,
    bool? isFailure,
    ErrorOnyx? error,
  }) {
    return BudgetServiceModel(
      value: value ?? this.value,
      isSuccess: isSuccess ?? this.isSuccess,
      isFailure: isFailure ?? this.isFailure,
      error: error ?? this.error,
    );
  }

  factory BudgetServiceModel.fromJson(Map<String, dynamic> json) {
    return BudgetServiceModel(
      value: (json['value'] as List).map((i) => Budget.fromJson(i)).toList(),
      isSuccess: json['isSuccess'] as bool,
      isFailure: json['isFailure'] as bool,
      error: ErrorOnyx.fromJson(json['error']),
    );
  }

  @override
  String toString() {
    return 'BudgetServiceModel(value: $value, isSuccess: $isSuccess, isFailure: $isFailure, error: $error)';
  }

  @override
  bool operator ==(covariant BudgetServiceModel other) {
    if (identical(this, other)) return true;

    return listEquals(other.value, value) &&
        other.isSuccess == isSuccess &&
        other.isFailure == isFailure &&
        other.error == error;
  }

  @override
  int get hashCode {
    return value.hashCode ^
        isSuccess.hashCode ^
        isFailure.hashCode ^
        error.hashCode;
  }
}

class Budget {
  final String id;
  final String name;
  final String currency;
  final List<String> userIds;
  final List<Account> accounts;
  final List<BudgetCategory> categories;
  final List<Counterparty> counterparties;

  const Budget({
    required this.id,
    required this.name,
    required this.currency,
    required this.userIds,
    required this.accounts,
    required this.categories,
    required this.counterparties,
  });

  factory Budget.fromJson(Map<String, dynamic> json) {
    return Budget(
      id: json['id'] as String,
      name: json['name'] as String,
      currency: json['currency'] as String,
      userIds: [],
      accounts: [],
      categories: [],
      counterparties: [],
    );
  }
}

class BudgetNotifier extends AsyncNotifier<BudgetServiceModel> {
  @override
  FutureOr<BudgetServiceModel> build() {
    String token = ref.watch(userToken.notifier).state;

    if (token.isEmpty) {
      return BudgetServiceModel(
        value: [],
        isSuccess: false,
        isFailure: true,
        error: const ErrorOnyx(message: 'Token is empty', code: "401"),
      );
    }
    final result = ref.watch(budgetServiceProvider).getBudgets(token);
    log("BudgetNotifier build result $result");
    return result;
  }
}
