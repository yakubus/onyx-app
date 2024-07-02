import 'dart:convert';
import 'dart:developer';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/config.dart';
import 'package:onyx_app/services/budget/budget.dart';
import 'package:http/http.dart' as http;

final budgetServiceProvider = Provider((ref) => BudgetService());

class BudgetService {
  final String apiUrl = '${Config.API_URL}/budgets';

  Future<BudgetServiceModel> getBudgets(String token) async {
    final response = await http.get(
      Uri.parse(apiUrl),
      headers: {
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      log('resmpne budget service ${response.body}');
      return BudgetServiceModel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('Failed to load budgets');
    }
  }
}
