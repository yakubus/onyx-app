import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/models/currency_dict.dart';
import 'package:onyx_app/services/user/user.dart';
import 'package:onyx_app/services/user/user_repo.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

final selectedCurrencyProvider = StateProvider<String>((ref) => "");

class RegisterUserDialog extends HookConsumerWidget {
  const RegisterUserDialog({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final emailController = useTextEditingController();
    final passwordController = useTextEditingController();
    final nameController = useTextEditingController();
    return ClipRRect(
      borderRadius: BorderRadius.circular(40),
      child: ShadDialog(
          title: Text(AppLocalizations.of(context)!.registrations),
          content: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.end,
              children: [
                const SizedBox(
                  height: 25,
                ),
                Row(
                  children: [
                    Expanded(
                      child: Text(AppLocalizations.of(context)!.user_name),
                    ),
                    const SizedBox(width: 16),
                    Expanded(
                      child: ShadInput(
                        controller: nameController,
                      ),
                    ),
                  ],
                ),
                const SizedBox(
                  height: 10,
                ),
                Row(
                  children: [
                    const Expanded(
                      child: Text("email"),
                    ),
                    const SizedBox(width: 16),
                    Expanded(
                      child: ShadInput(
                        controller: emailController,
                      ),
                    ),
                  ],
                ),
                const SizedBox(
                  height: 10,
                ),
                Row(
                  children: [
                    Expanded(
                      child: Text(AppLocalizations.of(context)!.password),
                    ),
                    const SizedBox(width: 16),
                    Expanded(
                      child: ShadInput(
                        controller: passwordController,
                      ),
                    ),
                  ],
                ),
                const SizedBox(
                  height: 10,
                ),
                Row(
                  children: [
                    Expanded(
                      child: Text(AppLocalizations.of(context)!.currency),
                    ),
                    const SizedBox(width: 16),
                    Expanded(
                      child: ShadSelect<String>(
                        placeholder:
                            Text(AppLocalizations.of(context)!.select_currency),
                        options: [
                          Padding(
                            padding: const EdgeInsets.fromLTRB(32, 6, 6, 6),
                            child: Text(
                              AppLocalizations.of(context)!.currency,
                              textAlign: TextAlign.start,
                            ),
                          ),
                          ...currencyDict.entries
                              .map((e) => ShadOption(
                                  value: e.key, child: Text(e.value)))
                              .toList(),
                        ],
                        selectedOptionBuilder: (context, value) =>
                            Text(currencyDict[value]!),
                        onChanged: (value) {
                          ref.read(selectedCurrencyProvider.notifier).state =
                              value;
                        },
                      ),
                    )
                  ],
                ),
                const SizedBox(
                  height: 10,
                ),
                ShadButton.secondary(
                  text: Text(AppLocalizations.of(context)!.register),
                  onPressed: () {
                    final selectedCurrency =
                        ref.read(selectedCurrencyProvider.notifier).state;

                    ref.watch(userServiceProvider).registerUser(
                        emailController.text,
                        passwordController.text,
                        selectedCurrency,
                        nameController.text);

                    ref.read(userToken.notifier).state = ref
                            .watch(userDataServiceProvider.notifier)
                            .state
                            .value
                            ?.accessToken ??
                        '';
                  },
                )
              ])),
    );
  }
}
