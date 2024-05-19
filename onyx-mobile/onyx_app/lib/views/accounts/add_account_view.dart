import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/models/money.dart';
import 'package:onyx_app/views/accounts/account_repo.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:onyx_app/views/accounts/accounts.dart';

class AddAccountView extends HookConsumerWidget {
  const AddAccountView({key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final accountNameController = useTextEditingController();
    final accountBalanceController = useTextEditingController();
    final accountCurrencyController = useTextEditingController();
    String? accountType;
    return Scaffold(
      body: Column(
        children: [
          SizedBox(height: 50, width: double.infinity),
          Row(
            children: [
              Expanded(
                child: TextField(
                  decoration: InputDecoration(
                    border: const OutlineInputBorder(),
                    labelText: AppLocalizations.of(context)!.account_name,
                    hintText: AppLocalizations.of(context)!.account_name,
                  ),
                  controller: accountNameController,
                ),
              ),
            ],
          ),
          Row(
            children: [
              Expanded(
                  child: TextField(
                decoration: InputDecoration(
                  border: const OutlineInputBorder(),
                  labelText: AppLocalizations.of(context)!.balance,
                  hintText: AppLocalizations.of(context)!.balance,
                ),
                controller: accountBalanceController,
              )),
              Expanded(
                child: TextField(
                  decoration: InputDecoration(
                    border: const OutlineInputBorder(),
                    labelText: AppLocalizations.of(context)!.currency,
                    hintText: AppLocalizations.of(context)!.currency,
                  ),
                  controller: accountCurrencyController,
                ),
              )
            ],
          ),
          Row(
            children: [
              Expanded(
                child: DropdownButton(
                    value:
                        accountType ?? AppLocalizations.of(context)!.checking,
                    items: [
                      DropdownMenuItem(
                          value: AppLocalizations.of(context)!.checking,
                          child: Text(AppLocalizations.of(context)!.checking)),
                      DropdownMenuItem(
                          value: AppLocalizations.of(context)!.saving,
                          child: Text(AppLocalizations.of(context)!.saving)),
                    ],
                    onChanged: (String? value) {
                      accountType = value;
                    }),
              )
            ],
          ),
          TextButton(
            onPressed: () {
              ref.watch(accountRepoProvider).addAccount(Account(
                  name: accountCurrencyController.text,
                  balance: Money(
                      currency: accountCurrencyController.text,
                      amount: double.parse(
                          accountBalanceController.text.replaceAll(',', '.'))),
                  type: accountType ?? AppLocalizations.of(context)!.checking));
            },
            child: Text(AppLocalizations.of(context)!.add),
          )
        ],
      ),
    );
  }
}
