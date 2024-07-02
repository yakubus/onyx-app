import 'dart:async';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/services/budget/budget.dart';
import 'package:onyx_app/views/budget_palns/budget_list.dart';
import 'package:onyx_app/widgets/appbar.dart';

class BudgetPlansView extends HookConsumerWidget {
  const BudgetPlansView({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final budgetData = ref.watch(budgetServiceDataProvider);

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
            Text(AppLocalizations.of(context)!.budget_view_title_desctiption,
                textAlign: TextAlign.center),
            BudgetList(),
          ],
        ),
      ),
    );
  }
}
