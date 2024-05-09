// ignore_for_file: public_member_api_docs, sort_constructors_first

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/mock.dart';

import 'package:onyx_app/views/accounts/accounts.dart';

final accountRepoProvider = Provider((ref) => AccountRepo(ref));

class AccountRepo {
  final Ref ref;
  const AccountRepo(
    this.ref,
  );

  Future<List<Account>> getAccounts() async {
    return Mocks.accountsMock;
  }

  Future<void> addAccount(Account account) async {}

  Future<void> updateAccount(Account account) async {}

  Future<void> deleteAccount(Account account) async {}

  Future<List<String>> getAllBalances() async {
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
  }

  Future<List<String>> getAccountNames() async {
    final accounts = await getAccounts();
    return accounts.map((account) => account.name).toList();
  }
}
