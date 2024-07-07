import 'package:flutter/material.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

ShadToast createLoginErrorToast(BuildContext context) {
  return ShadToast(
    title: Text(
      AppLocalizations.of(context)!.login_error,
      style: const TextStyle(color: Colors.red),
    ),
    description: Text(
      AppLocalizations.of(context)!.login_error_desc,
      style: const TextStyle(color: Colors.red),
    ),
    action: ShadButton.outline(
      text: Text(AppLocalizations.of(context)!.undo),
      onPressed: () => ShadToaster.of(context).hide(),
    ),
  );
}
