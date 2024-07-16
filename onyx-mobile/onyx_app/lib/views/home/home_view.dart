import 'package:google_fonts/google_fonts.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/themes/logo.dart';
import 'package:onyx_app/widgets/login_register/log_in_dialog.dart';
import 'package:onyx_app/themes/scafold_onyx.dart';
import 'package:onyx_app/widgets/appbar/appbar.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:onyx_app/widgets/login_register/register_user.dart';
import 'package:shadcn_ui/shadcn_ui.dart';

class Home extends HookConsumerWidget {
  const Home({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return ScafoldOnyx(
      appBar: const DefaultAppBar(
        title: "ONYX",
      ),
      body: MediaQuery.removeViewInsets(
        removeTop: true,
        context: context,
        child: SingleChildScrollView(
          child: ShadResizablePanelGroup(
            axis: Axis.vertical,
            height: MediaQuery.of(context).size.height * 0.8,
            children: [
              ShadResizablePanel(
                defaultSize: MediaQuery.of(context).size.height * 0.3,
                minSize: MediaQuery.of(context).size.height * 0.3,
                child: Center(
                  child: SizedBox(
                    height: 150,
                    width: 150,
                    child: DecoratedBox(
                      decoration: BoxDecoration(
                        color: Theme.of(context).colorScheme.secondary,
                      ),
                      child: const Center(
                        child: Column(
                          children: [
                            SizedBox(height: 23),
                            Logo(size: 70),
                          ],
                        ),
                      ),
                    ),
                  ),
                ),
              ),
              ShadResizablePanel(
                defaultSize: MediaQuery.of(context).size.height * 0.55,
                minSize: MediaQuery.of(context).size.height * 0.55,
                child: Container(
                  decoration: const BoxDecoration(
                    image: DecorationImage(
                      image: AssetImage('lib\\assets\\home_bg.png'),
                      fit: BoxFit.cover,
                    ),
                  ),
                  child: Column(
                    children: [
                      const SizedBox(height: 50),
                      Text(AppLocalizations.of(context)!.start_tekst,
                          textAlign: TextAlign.center,
                          style: GoogleFonts.lato(fontSize: 35)),
                      const SizedBox(height: 70),
                      if (ref.watch(isLogged) != true)
                        ShadButton.secondary(
                          size: ShadButtonSize.lg,
                          text: Text(AppLocalizations.of(context)!.login,
                              style: GoogleFonts.lato(
                                  fontSize: 20,
                                  color: Colors.white,
                                  fontWeight: FontWeight.bold)),
                          onPressed: () {
                            showDialog(
                              context: context,
                              builder: (context) {
                                return const LogInDialog();
                              },
                            );
                          },
                        ),
                      if (ref.watch(isLogged) != true)
                        ShadButton.link(
                          text: Text(
                              AppLocalizations.of(context)!.registrations,
                              style: GoogleFonts.lato(
                                  fontSize: 15, fontWeight: FontWeight.bold)),
                          onPressed: () {
                            showDialog(
                              context: context,
                              builder: (context) {
                                return const RegisterUserDialog();
                              },
                            );
                          },
                        )
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
