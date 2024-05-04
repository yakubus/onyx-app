import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';

class StatisticTile extends HookConsumerWidget {
  const StatisticTile({
    super.key,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return SizedBox(
      height: 100,
      child: InkWell(
        onTap: () {
          context.go('/statistics');
        },
        child: Card(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.assessment_rounded),
              const SizedBox(
                height: 10,
              ),
              Text(AppLocalizations.of(context)!.statistic),
            ],
          ),
        ),
      ),
    );
  }
}
