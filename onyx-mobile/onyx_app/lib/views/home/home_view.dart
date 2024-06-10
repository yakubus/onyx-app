import 'package:onyx_app/widgets/log_in_dialog.dart';
import 'package:onyx_app/themes/scafold_onyx.dart';
import 'package:onyx_app/widgets/appbar.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class Home extends HookConsumerWidget {
  const Home({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return ScafoldOnyx(
      appBar: const DefaultAppBar(
        title: "ONYX",
      ),
      body: Column(
        children: [
          ShadButton(
            text: Text(AppLocalizations.of(context)!.login),
            onPressed: () {
              showDialog(
                context: context,
                builder: (context) {
                  return const LogInDialog();
                },
              );
            },
          )
        ],
      ),
    );
  }
}
