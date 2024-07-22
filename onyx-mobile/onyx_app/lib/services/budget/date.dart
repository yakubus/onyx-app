import 'package:hooks_riverpod/hooks_riverpod.dart';

final dateProvider = StateNotifierProvider<DateNotifier, DateTime>(
  (ref) => DateNotifier(),
);

class DateNotifier extends StateNotifier<DateTime> {
  DateNotifier() : super(DateTime.now());

  void setDate(DateTime date) {
    state = date;
  }

  void incrementMonth() {
    state = DateTime(state.year, state.month + 1, state.day);
  }

  void decrementMonth() {
    state = DateTime(state.year, state.month - 1, state.day);
  }
}
