import 'package:onyx_app/main.dart';
import 'package:onyx_app/models/language_dict.dart';
import 'package:onyx_app/services/app_settings/settings.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

import 'package:shadcn_ui/shadcn_ui.dart';

class SettingsDialog extends HookConsumerWidget {
  const SettingsDialog({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final settings = ref.watch(settingsProvider);
    final SettingsData currentSettings = settings.maybeWhen(
        orElse: () => const SettingsData(language: "en", darkMode: false),
        data: (data) => data);
    return ClipRRect(
      borderRadius: BorderRadius.circular(40),
      child: ShadDialog(
        title: Text(AppLocalizations.of(context)!.settings),
        content: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const SizedBox(
                height: 25,
              ),
              ShadSwitch(
                value: currentSettings.darkMode,
                onChanged: (v) {
                  ref.read(settingsProvider.notifier).saveSettings(SettingsData(
                      language: currentSettings.language, darkMode: v));
                },
                label: Text(AppLocalizations.of(context)!.dark_mode),
              ),
              const SizedBox(
                height: 10,
              ),
              Row(
                children: [
                  Expanded(
                    child: Text(
                      AppLocalizations.of(context)!.language,
                    ),
                  ),
                  Expanded(
                    child: ShadSelect<String>(
                      placeholder:
                          Text(AppLocalizations.of(context)!.select_language),
                      options: [
                        ...languageDict.entries
                            .map((e) =>
                                ShadOption(value: e.key, child: Text(e.value)))
                            .toList(),
                      ],
                      selectedOptionBuilder: (context, value) =>
                          Text(languageDict[value]!),
                      onChanged: (String? newValue) {
                        ref.read(settingsProvider.notifier).saveSettings(
                            SettingsData(
                                language: newValue ?? currentSettings.language,
                                darkMode: currentSettings.darkMode));
                      },
                    ),
                  )
                ],
              ),
            ]),
      ),
    );
  }
}
