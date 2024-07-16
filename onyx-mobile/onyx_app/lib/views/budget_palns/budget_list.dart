// ignore_for_file: public_member_api_docs, sort_constructors_first

import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

import 'package:onyx_app/main.dart';
import 'package:onyx_app/services/budget/budget.dart';

final headings = [
  'name',
  'currency',
];

class BudgetList extends HookConsumerWidget {
  final AsyncValue<BudgetServiceModel> budget;
  const BudgetList({
    super.key,
    required this.budget,
  });

  @override
  Widget build(
    BuildContext context,
    WidgetRef ref,
  ) {
    return budget.when(
      data: (budgetServiceModel) {
        final budgetList = budgetServiceModel.value;
        return Center(
          child: ConstrainedBox(
            constraints: BoxConstraints(
              maxWidth: MediaQuery.of(context).size.width * 0.95,
              maxHeight: MediaQuery.of(context).size.height * 0.5,
            ),
            child: ListView.builder(
              itemCount: budgetList.length,
              itemBuilder: (context, index) {
                final budget = budgetList[index];
                return Column(
                  children: [
                    InkWell(
                      onTap: () {
                        context.go('/budget');
                      },
                      child: ShadCard(
                        content: Padding(
                          padding: const EdgeInsets.all(2.0),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Expanded(
                                child: Text(
                                  budget.name,
                                  style: const TextStyle(
                                    fontWeight: FontWeight.w500,
                                    fontSize: 16,
                                  ),
                                ),
                              ),
                              const SizedBox(width: 50),
                              Expanded(
                                child: Text(
                                  budget.currency,
                                  style: const TextStyle(fontSize: 14),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                  ],
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
