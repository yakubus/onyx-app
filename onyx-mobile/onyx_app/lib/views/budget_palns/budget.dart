import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/views/budget_palns/month_year_picker.dart';
import 'package:onyx_app/widgets/appbar/appbar.dart';

class BudgetView extends HookConsumerWidget {
  const BudgetView({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Scaffold(
      appBar: const DefaultAppBar(title: ""),
      body: MediaQuery.removeViewInsets(
        removeTop: true,
        context: context,
        child: const SingleChildScrollView(
          child: Column(
            children: [
              MonthYearPicker(),
            ],
          ),
        ),
      ),
    );
  }
}

class AppLocalizations {}
