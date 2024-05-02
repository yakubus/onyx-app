import 'package:firebase_kurs/views/home/goals_tile.dart';
import 'package:firebase_kurs/views/home/statistic_tile.dart';
import 'package:firebase_kurs/views/home/transaction_tile.dart';
import 'package:firebase_kurs/views/home/budget_plans_tile.dart';
import 'package:firebase_kurs/views/home/account_tile.dart';
import 'package:firebase_kurs/widgets/appbar.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';

class Home extends HookConsumerWidget {
  const Home({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Scaffold(
      appBar: const DefaultAppBar(title: 'Onyx'),
      body: ListView(
        children: const [
          Row(
            children: [
              Expanded(
                child: TransactionTile(),
              ),
              Expanded(
                child: BudgetPlansTile(),
              ),
            ],
          ),
          AccountTile(),
          Row(
            children: [
              Expanded(
                child: GoalsTile(),
              ),
              Expanded(
                child: StatisticTile(),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
