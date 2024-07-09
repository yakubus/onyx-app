import 'package:flutter/material.dart';
import 'package:flutter_hooks/flutter_hooks.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/main.dart';
import 'package:onyx_app/widgets/login_register/login_error_toast.dart';
import 'package:shadcn_ui/shadcn_ui.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class LogInDialog extends HookConsumerWidget {
  const LogInDialog({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final isLoading = useState(false);
    final emailController = useTextEditingController();
    final passwordController = useTextEditingController();
    return ClipRRect(
      borderRadius: BorderRadius.circular(40),
      child: ShadDialog(
        title: Text(AppLocalizations.of(context)!.login),
        content: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            const SizedBox(
              height: 25,
            ),
            Row(
              children: [
                const Expanded(
                  child: Text("email"),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: ShadInput(
                    controller: emailController,
                  ),
                ),
              ],
            ),
            const SizedBox(
              height: 10,
            ),
            Row(
              children: [
                Expanded(
                  child: Text(AppLocalizations.of(context)!.password),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: ShadInput(
                    controller: passwordController,
                  ),
                ),
              ],
            ),
            const SizedBox(
              height: 10,
            ),
            ShadButton.secondary(
              text: isLoading.value
                  ? const CircularProgressIndicator()
                  : Text(AppLocalizations.of(context)!.login),
              onPressed: isLoading.value
                  ? null
                  : () async {
                      isLoading.value = true;

                      try {
                        await ref.read(userServiceDataProvider.notifier).login(
                            emailController.text, passwordController.text);

                        if (ref.read(isLogged)) {
                          if (context.mounted) {
                            Navigator.of(context).pop();
                          }
                        } else {
                          if (context.mounted) {
                            ShadToaster.of(context)
                                .show(createLoginErrorToast(context));
                          }
                        }
                      } catch (e) {
                        if (context.mounted) {
                          ShadToaster.of(context)
                              .show(createLoginErrorToast(context));
                        }
                      } finally {
                        isLoading.value = false;
                      }
                    },
            ),
          ],
        ),
      ),
    );
  }
}
