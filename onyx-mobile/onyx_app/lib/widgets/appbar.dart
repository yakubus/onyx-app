import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/widgets/login_register/log_in_dialog.dart';

import 'package:onyx_app/widgets/main_menu/main_menu.dart';
import 'package:onyx_app/widgets/setings_view.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class DefaultAppBar extends ConsumerWidget implements PreferredSizeWidget {
  final String title;
  const DefaultAppBar({
    Key? key,
    required this.title,
  }) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final darkMode = ref.read(settingsProvider).value?.darkMode ?? false;
    final bgcolor =
        darkMode ? const Color.fromRGBO(52, 85, 74, 1.000) : Colors.white;
    log(" is Logged ${ref.watch(isLogged.notifier).state}");
    return Padding(
      padding: const EdgeInsets.all(5),
      child: AppBar(
        backgroundColor: Theme.of(context).colorScheme.primary,
        title: Text(title),
        leading: IconButton(
          icon: const Icon(Icons.menu),
          onPressed: () => showShadSheet(
            side: ShadSheetSide.left,
            context: context,
            builder: (context) => const MainMenu(),
          ),
        ),
        actions: [
          PopupMenuButton(
            color: bgcolor,
            child: const ShadAvatar(
              'https://app.requestly.io/delay/2000/avatars.githubusercontent.com/u/124599?v=4',
              placeholder: Text('CN'),
            ),
            itemBuilder: (BuildContext context) => <PopupMenuEntry>[
              PopupMenuItem(
                child: Text(
                  AppLocalizations.of(context)!.profile,
                ),
                onTap: () {},
              ),
              PopupMenuItem(
                child: Text(
                  AppLocalizations.of(context)!.billing,
                ),
                onTap: () {},
              ),
              PopupMenuItem(
                child: Text(
                  AppLocalizations.of(context)!.team,
                ),
                onTap: () {},
              ),
              PopupMenuItem(
                child: Text(
                  AppLocalizations.of(context)!.subscription,
                ),
                onTap: () {},
              ),
              PopupMenuItem(
                child: Text(
                  AppLocalizations.of(context)!.settings,
                ),
                onTap: () {
                  showDialog(
                    context: context,
                    builder: (context) {
                      return const SettingsDialog();
                    },
                  );
                },
              ),
              if (ref.watch(isLogged) == true)
                PopupMenuItem(
                  child: Text(
                    AppLocalizations.of(context)!.logout,
                  ),
                  onTap: () {
                    ref.watch(userServiceDataProvider.notifier).userLogout();
                    context.go('/');
                  },
                ),
              if (ref.watch(isLogged) != true)
                PopupMenuItem(
                  child: Text(
                    AppLocalizations.of(context)!.login,
                  ),
                  onTap: () {
                    showDialog(
                      context: context,
                      builder: (context) {
                        return const LogInDialog();
                      },
                    );
                  },
                )
            ],
          ),
        ],
      ),
    );
  }

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}
