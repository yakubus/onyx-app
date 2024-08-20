import 'package:flutter_hooks/flutter_hooks.dart';

import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/models/currency_dict.dart';

import 'package:shadcn_ui/shadcn_ui.dart';

class AddBudgetDialog extends HookConsumerWidget {
  const AddBudgetDialog({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final budgetNameController = useTextEditingController();
    String budgetCurrency = "PLN";
    return ClipRRect(
        borderRadius: BorderRadius.circular(40),
        child: ShadDialog(
            title: Row(children: [
              Text(AppLocalizations.of(context)!.add_budget),
              const Spacer(),
              Align(
                alignment: Alignment.topRight,
                child: IconButton(
                  icon: const Icon(Icons.close),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                ),
              ),
            ]),
            content: MediaQuery.removeViewInsets(
              removeTop: true,
              context: context,
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    Row(
                      children: [
                        Expanded(
                          child: Text(AppLocalizations.of(context)!.name),
                        ),
                        const SizedBox(width: 16),
                        Expanded(
                          child: ShadInput(
                            controller: budgetNameController,
                          ),
                        ),
                      ],
                    ),
                    Row(
                      children: [
                        Expanded(
                          child: Text(AppLocalizations.of(context)!.currency),
                        ),
                        const SizedBox(width: 16),
                        Expanded(
                          child: ShadSelect<String>(
                            placeholder: Text(
                                AppLocalizations.of(context)!.select_currency),
                            options: [
                              Padding(
                                padding: const EdgeInsets.fromLTRB(32, 6, 6, 6),
                                child: Text(
                                  AppLocalizations.of(context)!.currency,
                                  textAlign: TextAlign.start,
                                ),
                              ),
                              ...currencyDict.entries
                                  .map((e) => ShadOption(
                                      value: e.key, child: Text(e.value)))
                                  .toList(),
                            ],
                            selectedOptionBuilder: (context, value) =>
                                Text(currencyDict[value]!),
                            onChanged: (value) {
                              budgetCurrency = value;
                            },
                          ),
                        )
                      ],
                    ),
                    const SizedBox(
                      height: 10,
                    ),
                    ShadButton.link(
                      text: Text(AppLocalizations.of(context)!.add,
                          style: GoogleFonts.lato(
                              fontSize: 15, fontWeight: FontWeight.bold)),
                      onPressed: () {
                        ref.read(budgetServiceDataProvider.notifier).addBudget(
                            budgetNameController.text, budgetCurrency);
                      },
                    )
                  ],
                ),
              ),
            )));
  }
}
