import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';

import 'package:onyx_app/themes/logo.dart';
import 'package:onyx_app/widgets/navigate_item.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class MainMenu extends HookConsumerWidget {
  const MainMenu({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return ShadSheet(
      constraints: BoxConstraints(
          maxWidth: MediaQuery.of(context).size.width * 0.6,
          minWidth: MediaQuery.of(context).size.width * 0.6),
      title: const Center(
        child: Column(
          children: [
            SizedBox(height: 50),
            Logo(size: 50),
          ],
        ),
      ),
      content: Column(
        children: [
          const SizedBox(height: 50),
          const Divider(
            color: Color.fromRGBO(96, 133, 121, 1),
            thickness: 1,
          ),
          NavigateItem(
            itemText: AppLocalizations.of(context)!.account,
            route: "account",
          ),
          const SizedBox(height: 8),
          const Divider(
            color: Color.fromRGBO(96, 133, 121, 1),
            thickness: 1,
          ),
          NavigateItem(
            itemText: AppLocalizations.of(context)!.budget_plans,
            route: "budget_plans",
          ),
          const SizedBox(height: 8),
          const Divider(
            color: Color.fromRGBO(96, 133, 121, 1),
            thickness: 1,
          ),
          Align(
            alignment: Alignment.bottomRight,
            child: IconButton(
              icon: const Icon(Icons.arrow_back, color: Colors.white),
              onPressed: () {
                Navigator.pop(context);
              },
            ),
          ),
        ],
      ),
    );
  }
}
