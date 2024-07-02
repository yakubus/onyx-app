// ignore_for_file: public_member_api_docs, sort_constructors_first

import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/services/budget/budget.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

final headings = [
  'name',
  'currency',
];

class BudgetList extends HookConsumerWidget {
  const BudgetList({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final budget = ref.watch(budgetServiceDataProvider);

    return budget.when(
      data: (budgetServiceModel) {
        final budgetList = budgetServiceModel.value;
        return ConstrainedBox(
          constraints: BoxConstraints(
              maxWidth: MediaQuery.of(context).size.width * 0.9,
              maxHeight: MediaQuery.of(context).size.height * 0.5),
          child: Center(
            child: ShadTable(
              columnCount: headings.length,
              rowCount: budgetList.length,
              header: (context, column) {
                final isLast = column == headings.length - 1;
                return ShadTableCell.header(
                  alignment: isLast ? Alignment.centerRight : null,
                  child: Text(headings[column]),
                );
              },
              builder: (context, index) {
                final budget = budgetList[index.row];
                final cellValue = [
                  budget.name,
                  budget.currency,
                ];
                return ShadTableCell(
                  child: Text(
                    cellValue[index.column],
                    style: index.column == 0
                        ? const TextStyle(fontWeight: FontWeight.w500)
                        : null,
                  ),
                );
              },
            ),
          ),
        );
      },
      loading: () => const Center(child: CircularProgressIndicator()),
      error: (error, stackTrace) =>
          Center(child: Text('Error: ${error.toString()}')),
    );
  }
}
