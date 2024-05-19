import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/views/accounts/accounts.dart';

final accountProvider =
    AsyncNotifierProvider<AccountListNotifier, List<Account>>(
        AccountListNotifier.new);

class AccountSummary extends HookConsumerWidget {
  const AccountSummary({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Card(
        child: Column(
      children: [],
    ));
  }
}
