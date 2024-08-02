import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/services/budget/date.dart';
import 'package:onyx_app/widgets/data_picker/date_selector.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class MonthYearPicker extends HookConsumerWidget {
  const MonthYearPicker({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final selectedDate = ref.watch(dateProvider);
    final dateNotifier = ref.read(dateProvider.notifier);

    return Row(
      children: [
        IconButton(
          icon: const Icon(Icons.arrow_left),
          onPressed: () {
            dateNotifier.decrementMonth();
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
              "date",
              style: const TextStyle(fontSize: 16),
            ),
          ),
        ),
        IconButton(
          icon: const Icon(Icons.arrow_right),
          onPressed: () {
            dateNotifier.incrementMonth();
          },
        ),
      ],
    );
  }
}
