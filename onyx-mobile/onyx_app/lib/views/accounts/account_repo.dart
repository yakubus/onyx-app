// ignore_for_file: public_member_api_docs, sort_constructors_first

import 'dart:convert';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:http/http.dart' as http;
import 'package:onyx_app/config.dart';
import 'package:onyx_app/models/money.dart';

import 'package:onyx_app/views/accounts/accounts.dart';

final accountRepoProvider = Provider((ref) => AccountRepo(ref));

class AccountRepo {
  final Ref ref;
  const AccountRepo(
    this.ref,
  );

  Future<List<Account>> getAccounts() async {
    try {
      final response = await http.get(
        Uri.parse('${Config.API_URL_BD}/accounts'),
      );
      if (response.statusCode == 200) {
        final data = json.decode(response.body);
        final List<Account> accounts = [];
        for (var item in data['value']) {
          accounts.add(Account(
            id: item['id'],
            name: item['name'],
            balance: Money(
              currency: item['balance']['currency'],
              amount: item['balance']['amount'],
            ),
            type: item['type'],
          ));
        }
        return accounts;
      } else {
        throw Exception('Nie udało się pobrać danych z API.');
      }
    } catch (e) {
      throw Exception('Wystąpił błąd: $e');
    }
  }

  Future<void> addAccount(Account account) async {
    final response = await http.post(
      Uri.parse('${Config.API_URL_BD}/accounts'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(<String, dynamic>{
        'id': account.id,
        'name': account.name,
        'balance': {
          'amount': account.balance.amount,
          'currency': account.balance.currency,
        },
        'type': account.type,
      }),
    );
    if (response.statusCode != 200) {
      throw Exception('Failed to create account');
    }
  }
}

Future<void> updateAccount(Account account) async {}

Future<void> deleteAccount(Account account) async {}

  /*Future<List<String>> getAllBalances() async {
    List<String> currency = [];
    List<double> balance = [];
    List<String> result = [];
    final accounts = await getAccounts();
    accounts.map((account) {
      for (int i = 0; i < currency.length; i++) {
        if (currency[i] == account.balance.currency) {
          balance[i] += account.balance.amount;
          return;
        } else {
          currency.add(account.balance.currency);
          balance.add(account.balance.amount);
        }
      }
    });

    for (int i = 0; i < currency.length; i++) {
      result.add('${currency[i]} ${balance[i]}');
    }

    return result;
  }*/

 /* Future<List<String>> getAccountNames() async {
    final accounts = await getAccounts();
    return accounts.map((account) => account.name).toList();
  }*/
// Remove the incomplete code block
