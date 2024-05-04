import 'package:adaptive_theme/adaptive_theme.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/views/settings/settings.dart';
import 'package:onyx_app/widgets/appbar.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class SettingsView extends HookConsumerWidget {
  const SettingsView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final settings = ref.watch(settingsProvider);
    final SettingsData currentSettings = settings.maybeWhen(
        orElse: () => const SettingsData(language: "en", darkMode: false),
        data: (data) => data);
    return Scaffold(
      appBar: AppBar(
        title: const DefaultAppBar(title: "Settings"),
      ),
      body: Padding(
        padding: const EdgeInsets.all(30.0),
        child: ListView(
          children: [
            Row(children: [
              Expanded(
                child: Text(
                  AppLocalizations.of(context)!.dark_mode,
                  style: Theme.of(context).textTheme.titleLarge,
                ),
              ),
              Expanded(
                child: Switch(
                  value: currentSettings.darkMode,
                  onChanged: (value) {
                    ref.read(settingsProvider.notifier).saveSettings(
                        SettingsData(
                            language: currentSettings.language,
                            darkMode: value));
                    if (value) {
                      AdaptiveTheme.of(context).setDark();
                    } else {
                      AdaptiveTheme.of(context).setLight();
                    }
                  },
                ),
              ),
            ]),
            Row(children: [
              Expanded(
                child: Text(
                  AppLocalizations.of(context)!.language,
                  style: Theme.of(context).textTheme.titleLarge,
                ),
              ),
              Expanded(
                child: DropdownButton<String>(
                  value: currentSettings.language,
                  onChanged: (String? newValue) {
                    ref.read(settingsProvider.notifier).saveSettings(
                        SettingsData(
                            language: newValue ?? currentSettings.language,
                            darkMode: currentSettings.darkMode));
                  },
                  items: <String>['en', 'pl']
                      .map<DropdownMenuItem<String>>((String value) {
                    return DropdownMenuItem<String>(
                      value: value,
                      child: Text(value),
                    );
                  }).toList(),
                ),
              ),
            ]),
          ],
        ),
      ),
    );
  }
}
