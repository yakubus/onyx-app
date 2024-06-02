// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:async';
import 'dart:convert';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/models/money.dart';
import 'package:onyx_app/views/accounts/account_repo.dart';

enum AccountType { checking, savings }

class Account {
  String? id;
  String name;
  Money balance;
  String type;

  Account({
    this.id,
    required this.name,
    required this.balance,
    required this.type,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'name': name,
      'balance': balance.toMap(),
      'type': type,
    };
  }

  factory Account.fromMap(Map<String, dynamic> map) {
    return Account(
      id: (map["id"] ?? '') as String,
      name: (map["name"] ?? '') as String,
      balance: Money.fromMap((map["balance"] ?? Map<String, dynamic>.from({}))
          as Map<String, dynamic>),
      type: (map["type"] ?? '') as String,
    );
  }

  String toJson() => json.encode(toMap());

  factory Account.fromJson(String source) =>
      Account.fromMap(json.decode(source) as Map<String, dynamic>);
}

class AccountListNotifier extends AsyncNotifier<List<Account>> {
  @override
  FutureOr<List<Account>> build() {
    return ref.read(accountRepoProvider).getAccounts();
  }

  Future<void> refresh() async {
    state = await AsyncValue.guard(
        () => ref.watch(accountRepoProvider).getAccounts());
  }

  Future<void> addAccount(Account account) async {
    ref.read(accountRepoProvider).addAccount(account);
    refresh();
  }

  /*Future<void> updateAccount(Account account) async {
    ref.read(accountRepoProvider).updateAccount(account);
    refresh();
  }

  Future<void> deleteAccount(Account account) async {
    ref.read(accountRepoProvider).deleteAccount(account);
    refresh();
  }

  Future<List<String>> getAllBalances() async {
    return ref.read(accountRepoProvider).getAllBalances();
  }

  Future<List<String>> getAccountNames() async {
    return ref.read(accountRepoProvider).getAccountNames();
  }*/
}
