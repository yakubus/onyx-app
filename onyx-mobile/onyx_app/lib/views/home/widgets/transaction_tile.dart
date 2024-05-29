import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class TransactionTile extends HookConsumerWidget {
  const TransactionTile({
    super.key,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return SizedBox(
      height: 100,
      child: InkWell(
        onTap: () {
          context.go('/transactions');
        },
        child: ShadCard(
          content: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.credit_card_rounded),
              const SizedBox(
                height: 10,
              ),
              Text(AppLocalizations.of(context)!.transactions),
            ],
          ),
        ),
      ),
    );
  }
}
