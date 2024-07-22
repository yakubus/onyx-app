import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:intl/intl.dart';
import 'package:onyx_app/services/budget/date.dart';

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
          onTap: () async {
            final pickedDate = await showDatePicker(
              context: context,
              initialDate: selectedDate,
              firstDate: DateTime(2000),
              lastDate: DateTime(2100),
              initialDatePickerMode: DatePickerMode.year,
            );
            if (pickedDate != null) {
              dateNotifier.setDate(pickedDate);
            }
          },
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
            decoration: BoxDecoration(
              border: Border.all(color: Colors.grey),
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(
              DateFormat.yMMMM().format(selectedDate),
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
