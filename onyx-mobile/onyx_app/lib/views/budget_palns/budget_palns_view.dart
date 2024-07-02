import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:onyx_app/views/budget_palns/add_budget.dart';
import 'package:onyx_app/views/budget_palns/budget_list.dart';
import 'package:onyx_app/widgets/appbar.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class BudgetPlansView extends HookConsumerWidget {
  const BudgetPlansView({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Scaffold(
      appBar: DefaultAppBar(
        title: AppLocalizations.of(context)!.budget_plans,
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const SizedBox(height: 50),
            Text(AppLocalizations.of(context)!.budget_view_title,
                style: GoogleFonts.lato(fontSize: 30),
                textAlign: TextAlign.center),
            const SizedBox(height: 20),
            Text(AppLocalizations.of(context)!.budget_view_title_desctiption,
                textAlign: TextAlign.center),
            const SizedBox(height: 50),
            const BudgetList(),
            Container(
              padding: const EdgeInsets.all(10),
              child: ShadButton.outline(
                onPressed: () {
                  showDialog(
                    context: context,
                    builder: (context) {
                      return const AddBudgetDialog();
                    },
                  );
                },
                icon: const Icon(Icons.add),
              ),
            )
          ],
        ),
      ),
    );
  }
}
