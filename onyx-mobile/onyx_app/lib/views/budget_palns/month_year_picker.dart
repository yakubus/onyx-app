import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/widgets/data_picker/date_selector_service.dart';
import 'package:onyx_app/widgets/data_picker/date_selector.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class MonthYearPicker extends HookConsumerWidget {
  const MonthYearPicker({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Row(
      children: [
        IconButton(
          icon: const Icon(Icons.arrow_left),
          onPressed: () {
            ref.watch(budgetDataProvider).decrementMonth();
          },
        ),
        InkWell(
          onTap: () => showShadSheet(
            side: ShadSheetSide.left,
            context: context,
            builder: (context) => const DateSelector(),
          ),
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
            decoration: BoxDecoration(
              border: Border.all(color: Colors.grey),
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(
              ref.watch(budgetDataProvider).toString(),
              style: const TextStyle(fontSize: 16),
            ),
          ),
        ),
        IconButton(
          icon: const Icon(Icons.arrow_right),
          onPressed: () {
            ref.watch(budgetDataProvider).incrementMonth();
          },
        ),
      ],
    );
  }
}
