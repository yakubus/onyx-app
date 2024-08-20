// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:hooks_riverpod/hooks_riverpod.dart';

import 'package:onyx_app/models/month_date.dart';

final budgetDataProvider = StateProvider<BudgetDate>((ref) => BudgetDate(
    monthDate:
        MonthDate(month: DateTime.now().month, year: DateTime.now().year)));

class BudgetDate {
  final MonthDate monthDate;

  const BudgetDate({
    required this.monthDate,
  });

  void incrementYear() {
    monthDate.setYear(monthDate.year++);
  }

  void decrementYear() {
    if (monthDate.year == DateTime.now().year) {
      return;
    } else {
      monthDate.setYear(monthDate.year--);
    }
  }

  void incrementMonth() {
    if (monthDate.month == 12) {
      monthDate.setMonth(1);
      incrementYear();
    } else {
      monthDate.setMonth(monthDate.month++);
    }
  }

  void decrementMonth() {
    if (monthDate.month == 1) {
      monthDate.setMonth(12);
      decrementYear();
    } else {
      monthDate.setMonth(monthDate.month--);
    }
  }

  @override
  String toString() => 'BudgetDate(monthDate: $monthDate)';
}
