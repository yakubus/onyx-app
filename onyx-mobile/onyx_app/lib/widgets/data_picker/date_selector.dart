import 'package:hooks_riverpod/hooks_riverpod.dart';

import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter/material.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

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
