import 'package:onyx_app/views/accounts/accounts_view.dart';
import 'package:onyx_app/views/accounts/add_account_view.dart';
import 'package:onyx_app/views/budget_palns/budget_palns_view.dart';
import 'package:onyx_app/views/goals/goals_view.dart';
import 'package:onyx_app/views/home/home_view.dart';

import 'package:onyx_app/views/statistic/statistic.dart';
import 'package:onyx_app/views/transactions/transactions_view.dart';
import 'package:flutter/widgets.dart';
import 'package:go_router/go_router.dart';

final GoRouter goRouter = GoRouter(
  routes: <RouteBase>[
    GoRoute(
      path: '/',
      builder: (BuildContext context, GoRouterState state) {
        return const Home();
      },
      routes: <RouteBase>[
        GoRoute(
          path: 'account',
          builder: (BuildContext context, GoRouterState state) {
            return const AccountView();
          },
        ),
        GoRoute(
          path: 'budget_plans',
          builder: (BuildContext context, GoRouterState state) {
            return const BudgetPlansView();
          },
        ),
        GoRoute(
            path: 'statistics',
            builder: (BuildContext context, GoRouterState state) {
              return const StatisticView();
            }),
        GoRoute(
            path: 'transactions',
            builder: (BuildContext context, GoRouterState state) {
              return const TransactionView();
            }),
        GoRoute(
            path: 'goals',
            builder: (BuildContext context, GoRouterState state) {
              return const GoalsView();
            }),
        GoRoute(
            path: 'add_account',
            builder: (BuildContext context, GoRouterState state) {
              return const AddAccountView();
            }),
      ],
    ),
  ],
);
