import 'package:firebase_kurs/views/accounts/accounts_view.dart';
import 'package:firebase_kurs/views/budget_palns/budget_palns_view.dart';
import 'package:firebase_kurs/views/goals/goals_view.dart';
import 'package:firebase_kurs/views/home/home_view.dart';
import 'package:firebase_kurs/views/home/statistic_tile.dart';
import 'package:firebase_kurs/views/home/transaction_tile.dart';
import 'package:firebase_kurs/views/settings/setings_view.dart';
import 'package:firebase_kurs/views/statistic/statistic.dart';
import 'package:firebase_kurs/views/transactions/transactions_view.dart';
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
          path: 'settings',
          builder: (BuildContext context, GoRouterState state) {
            return const Settings();
          },
        ),
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
      ],
    ),
  ],
);
