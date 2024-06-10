import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/services/user/user_repo.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

final profile = [
  (title: 'Email', value: 'Alexandru'),
  (title: 'Username', value: 'nank1ro'),
];

class LogInDialog extends HookConsumerWidget {
  const LogInDialog({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final emailController = useTextEditingController();
    final passwordController = useTextEditingController();
    return ShadDialog(
        title: Text(AppLocalizations.of(context)!.login),
        content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
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
              ShadButton(
                text: Text(AppLocalizations.of(context)!.login),
                onPressed: () {
                  ref
                      .watch(userServiceProvider)
                      .loginUser(emailController.text, passwordController.text);
                },
              )
            ]));
  }
}
