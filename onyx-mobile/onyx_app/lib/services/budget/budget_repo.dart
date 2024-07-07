import 'dart:convert';
import 'dart:developer';

import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/config.dart';
import 'package:onyx_app/services/budget/budget.dart';
import 'package:http/http.dart' as http;

final budgetServiceProvider = Provider((ref) => BudgetService());
const String apiUrl = '${Config.API_URL}/budgets';

class BudgetService {
  Future<BudgetServiceModel> getBudgets(String token) async {
    final response = await http.get(
      Uri.parse(apiUrl),
      headers: {
        'Authorization': 'Bearer $token',
      },
    );
    if (response.statusCode == 200) {
      log('resumee budget service ${response.body}');
      return BudgetServiceModel.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('Failed to load budgets');
    }
  }

  Future<BudgetServiceModel> addBudget(
      String token, String name, String currency) async {
    final response = await http.post(
      Uri.parse(apiUrl),
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'budgetName': name,
        'budgetCurrency': currency,
      }),
    );
    if (response.statusCode == 200) {
      log('add budget service ${response.body}');
      return BudgetServiceModel.fromJson(jsonDecode(response.body));
    } else {
      log('add budget service ${response.body}');
      throw Exception('Failed to create budget');
    }
  }
}
