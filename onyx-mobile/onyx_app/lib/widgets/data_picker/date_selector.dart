import 'dart:developer';

import 'package:hooks_riverpod/hooks_riverpod.dart';

import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter/material.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

final currentYear = StateProvider<int>((ref) => DateTime.now().year);

class DateSelector extends HookConsumerWidget {
  const DateSelector({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return ClipRRect(
      child: ShadDialog(
        content: Column(
          children: [
            Row(
              children: [
                IconButton(
                  icon: const Icon(Icons.arrow_left),
                  onPressed: () {
                    log(ref.read(currentYear).toString());
                  },
                ),
                Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                  decoration: BoxDecoration(
                    border: Border.all(color: Colors.grey),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Text(
                    ref.read(currentYear).toString(),
                    style: const TextStyle(fontSize: 16),
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.arrow_right),
                  onPressed: () {},
                ),
              ],
            ),
            Row(
              children: [
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.jan),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.feb),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.mar),
                      onPressed: () {},
                    ),
                  ),
                )
              ],
            ),
            Row(
              children: [
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Center(
                        child: Text(
                          AppLocalizations.of(context)!.apr,
                        ),
                      ),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.may),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.jun),
                      onPressed: () {},
                    ),
                  ),
                )
              ],
            ),
            Row(
              children: [
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.jul),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.aug),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.sep),
                      onPressed: () {},
                    ),
                  ),
                )
              ],
            ),
            Row(
              children: [
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.oct),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.nov),
                      onPressed: () {},
                    ),
                  ),
                ),
                Expanded(
                  child: Center(
                    child: ShadButton.secondary(
                      text: Text(AppLocalizations.of(context)!.dec),
                      onPressed: () {},
                    ),
                  ),
                )
              ],
            )
          ],
        ),
      ),
    );
  }
}
