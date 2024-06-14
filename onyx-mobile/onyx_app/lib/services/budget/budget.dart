// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:async';
import 'dart:convert';

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

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'value': value.map((x) {
        return x.toMap();
      }).toList(growable: false),
      'isSuccess': isSuccess,
      'isFailure': isFailure,
      'error': error.toMap(),
    };
  }

  factory BudgetServiceModel.fromMap(Map<String, dynamic> map) {
    return BudgetServiceModel(
      value: List<Budget>.from(
        ((map['value'] ?? const <Budget>[]) as List).map<Budget>((x) {
          return Budget.fromMap(
              (x ?? Map<String, dynamic>.from({})) as Map<String, dynamic>);
        }),
      ),
      isSuccess: (map["isSuccess"] ?? false) as bool,
      isFailure: (map["isFailure"] ?? false) as bool,
      error: ErrorOnyx.fromMap((map["error"] ?? Map<String, dynamic>.from({}))
          as Map<String, dynamic>),
    );
  }

  String toJson() => json.encode(toMap());

  factory BudgetServiceModel.fromJson(String source) =>
      BudgetServiceModel.fromMap(json.decode(source) as Map<String, dynamic>);

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

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'name': name,
      'currency': currency,
      'userIds': userIds,
      'accounts': accounts.map((x) {
        return x.toMap();
      }).toList(growable: false),
      'categories': categories.map((x) {
        return x.toMap();
      }).toList(growable: false),
      'counterparties': counterparties.map((x) {
        return x.toMap();
      }).toList(growable: false),
    };
  }

  factory Budget.fromMap(Map<String, dynamic> map) {
    return Budget(
      id: (map["id"] ?? '') as String,
      name: (map["name"] ?? '') as String,
      currency: (map["currency"] ?? '') as String,
      userIds: List<String>.from(
        ((map['userIds'] ?? const <String>[]) as List<String>),
      ),
      accounts: List<Account>.from(
        ((map['accounts'] ?? const <Account>[]) as List).map<Account>((x) {
          return Account.fromMap(
              (x ?? Map<String, dynamic>.from({})) as Map<String, dynamic>);
        }),
      ),
      categories: List<BudgetCategory>.from(
        ((map['categories'] ?? const <BudgetCategory>[]) as List)
            .map<BudgetCategory>((x) {
          return BudgetCategory.fromMap(
              (x ?? Map<String, dynamic>.from({})) as Map<String, dynamic>);
        }),
      ),
      counterparties: List<Counterparty>.from(
        ((map['counterparties'] ?? const <Counterparty>[]) as List)
            .map<Counterparty>((x) {
          return Counterparty.fromMap(
              (x ?? Map<String, dynamic>.from({})) as Map<String, dynamic>);
        }),
      ),
    );
  }

  String toJson() => json.encode(toMap());

  factory Budget.fromJson(String source) =>
      Budget.fromMap(json.decode(source) as Map<String, dynamic>);
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
    return ref.watch(budgetServiceProvider).getBudgets(token);
  }
}
