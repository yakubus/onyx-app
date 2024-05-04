import 'package:onyx_app/views/home/widgets/goals_tile.dart';
import 'package:onyx_app/views/home/widgets/statistic_tile.dart';
import 'package:onyx_app/views/home/widgets/transaction_tile.dart';
import 'package:onyx_app/views/home/widgets/budget_plans_tile.dart';
import 'package:onyx_app/views/home/widgets/account_tile.dart';
import 'package:onyx_app/widgets/appbar.dart';
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
