import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
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
    return ShadDialog(
        //title: Text(AppLocalizations.of(context)!.log_in),
        content: const Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
          Row(
            children: [
              Expanded(
                child: Text("email"),
              ),
              SizedBox(width: 16),
              Expanded(
                child: ShadInput(),
              ),
            ],
          )
        ]));
  }
}
