import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';

class BudgetPlansTile extends StatelessWidget {
  const BudgetPlansTile({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 100,
      child: InkWell(
        onTap: () {
          context.go('/budget_plans');
        },
        child: Card(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.calendar_month),
              const SizedBox(
                height: 10,
              ),
              Text(AppLocalizations.of(context)!.budget_plans),
            ],
          ),
        ),
      ),
    );
  }
}
