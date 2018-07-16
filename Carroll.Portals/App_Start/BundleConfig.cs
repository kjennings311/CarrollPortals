using System.Web;
using System.Web.Optimization;

namespace Carroll.Portals
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/scripts/bootstrap.js",
            //          "~/scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/content/css").Include(
            //          "~/content/bootstrap.min.css",
            //          "~/content/animate.css",
            //          "~/content/style.css"
            //          ));

            //// Font Awesome icons
            //bundles.Add(new StyleBundle("~/font-awesome/css").Include(
            //          "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));
            //// Inspinia script
            //bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
            //          "~/scripts/app/inspinia.js"));

            //// SlimScroll
            //bundles.Add(new ScriptBundle("~/scripts/plugins/slimScroll").Include(
            //          "~/scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            //// jQuery plugins
            //bundles.Add(new ScriptBundle("~/scripts/plugins/metsiMenu").Include(
            //          "~/scripts/plugins/metisMenu/metisMenu.min.js"));

            //bundles.Add(new ScriptBundle("~/scripts/plugins/pace").Include(
            //          "~/scripts/plugins/pace/pace.min.js"));

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/content/css").Include(
                      "~/content/bootstrap.min.css",
                      "~/content/animate.css",
                      "~/content/style.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/scripts/jquery-3.1.1.min.js",
                        "~/scripts/jquery.easing.min.js"));

            // jQueryUI CSS
            bundles.Add(new ScriptBundle("~/content/plugins/jquery-ui/jqueryuiStyles").Include(
                        "~/content/plugins/jquery-ui/jquery-ui.min.css"));

            // jQueryUI 
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/scripts/plugins/jquery-ui/jquery-ui.min.js"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/scripts/bootstrap.min.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/scripts/plugins/metisMenu/metisMenu.min.js",
                      "~/scripts/plugins/pace/pace.min.js",
                      "~/scripts/app/inspinia.js",
                      "~/scripts/app/custom.js"
                      ));

            // Inspinia skin config script
            bundles.Add(new ScriptBundle("~/bundles/skinConfig").Include(
                      "~/scripts/app/skin.config.min.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/scripts/plugins/slimScroll").Include(
                      "~/scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            // Peity
            bundles.Add(new ScriptBundle("~/scripts/plugins/peity").Include(
                      "~/scripts/plugins/peity/jquery.peity.min.js"));

            // Video responsible
            bundles.Add(new ScriptBundle("~/scripts/plugins/videoResponsible").Include(
                      "~/scripts/plugins/video/responsible-video.js"));

            // Lightbox gallery css styles
            bundles.Add(new StyleBundle("~/content/plugins/blueimp/css/").Include(
                      "~/content/plugins/blueimp/css/blueimp-gallery.min.css"));

            // Lightbox gallery
            bundles.Add(new ScriptBundle("~/scripts/plugins/lightboxGallery").Include(
                      "~/scripts/plugins/blueimp/jquery.blueimp-gallery.min.js"));

            // Sparkline
            bundles.Add(new ScriptBundle("~/scripts/plugins/sparkline").Include(
                      "~/scripts/plugins/sparkline/jquery.sparkline.min.js"));

            // Morriss chart css styles
            bundles.Add(new StyleBundle("~/content/plugins/morrisStyles").Include(
                      "~/content/plugins/morris/morris-0.4.3.min.css"));

            // Morriss chart
            bundles.Add(new ScriptBundle("~/scripts/plugins/morris").Include(
                      "~/scripts/plugins/morris/raphael-2.1.0.min.js",
                      "~/scripts/plugins/morris/morris.js"));

            // Flot chart
            bundles.Add(new ScriptBundle("~/scripts/plugins/flot").Include(
                      "~/scripts/plugins/flot/jquery.flot.js",
                      "~/scripts/plugins/flot/jquery.flot.tooltip.min.js",
                      "~/scripts/plugins/flot/jquery.flot.resize.js",
                      "~/scripts/plugins/flot/jquery.flot.pie.js",
                      "~/scripts/plugins/flot/jquery.flot.time.js",
                      "~/scripts/plugins/flot/jquery.flot.spline.js"));

            // Rickshaw chart
            bundles.Add(new ScriptBundle("~/scripts/plugins/rickshaw").Include(
                      "~/scripts/plugins/rickshaw/vendor/d3.v3.js",
                      "~/scripts/plugins/rickshaw/rickshaw.min.js"));

            // ChartJS chart
            bundles.Add(new ScriptBundle("~/scripts/plugins/chartJs").Include(
                      "~/scripts/plugins/chartjs/Chart.min.js"));

            // iCheck css styles
            bundles.Add(new StyleBundle("~/content/plugins/iCheck/iCheckStyles").Include(
                      "~/content/plugins/iCheck/custom.css"));

            // iCheck
            bundles.Add(new ScriptBundle("~/scripts/plugins/iCheck").Include(
                      "~/scripts/plugins/iCheck/icheck.min.js"));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/content/plugins/dataTables/dataTablesStyles").Include(
                      "~/content/plugins/dataTables/datatables.min.css",
                      "~/content/plugins/dataTables/buttons.dataTables.min.css",
                      "~/content/plugins/dataTables/select.dataTables.min.css",
                      "~/content/plugins/dataTables/responsive.dataTables.min.css"


                      ));

            // dataTables 
            bundles.Add(new ScriptBundle("~/scripts/plugins/dataTables").Include(
                      "~/scripts/plugins/dataTables/datatables.min.js",
                       "~/scripts/plugins/dataTables/dataTables.buttons.min.js",
                       "~/scripts/plugins/dataTables/dataTables.select.min.js",
                       "~/scripts/plugins/dataTables/dataTables.responsive.min.js",
                       "~/scripts/plugins/dataTables/dataTables.fixedColumns.min.js",
                        "~/scripts/plugins/dataTables/dataTables.altEditor.js"
                      ));

            // jeditable 
            bundles.Add(new ScriptBundle("~/scripts/plugins/jeditable").Include(
                      "~/scripts/plugins/jeditable/jquery.jeditable.js"));

            // jqGrid styles
            bundles.Add(new StyleBundle("~/content/plugins/jqGrid/jqGridStyles").Include(
                      "~/content/plugins/jqGrid/ui.jqgrid.css"));

            // jqGrid 
            bundles.Add(new ScriptBundle("~/scripts/plugins/jqGrid").Include(
                      "~/scripts/plugins/jqGrid/i18n/grid.locale-en.js",
                      "~/scripts/plugins/jqGrid/jquery.jqGrid.min.js"));

            // codeEditor styles
            bundles.Add(new StyleBundle("~/content/plugins/codeEditorStyles").Include(
                      "~/content/plugins/codemirror/codemirror.css",
                      "~/content/plugins/codemirror/ambiance.css"));

            // codeEditor 
            bundles.Add(new ScriptBundle("~/scripts/plugins/codeEditor").Include(
                      "~/scripts/plugins/codemirror/codemirror.js",
                      "~/scripts/plugins/codemirror/mode/javascript/javascript.js"));

            // codeEditor 
            bundles.Add(new ScriptBundle("~/scripts/plugins/nestable").Include(
                      "~/scripts/plugins/nestable/jquery.nestable.js"));

            // validate 
            bundles.Add(new ScriptBundle("~/scripts/plugins/validate").Include(
                      "~/scripts/plugins/validate/jquery.validate.min.js"));

            // fullCalendar styles
            bundles.Add(new StyleBundle("~/content/plugins/fullCalendarStyles").Include(
                      "~/content/plugins/fullcalendar/fullcalendar.css"));

            // fullCalendar 
            bundles.Add(new ScriptBundle("~/scripts/plugins/fullCalendar").Include(
                      "~/scripts/plugins/fullcalendar/moment.min.js",
                      "~/scripts/plugins/fullcalendar/fullcalendar.min.js"));

            // vectorMap 
            bundles.Add(new ScriptBundle("~/scripts/plugins/vectorMap").Include(
                      "~/scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));

            // ionRange styles
            bundles.Add(new StyleBundle("~/content/plugins/ionRangeSlider/ionRangeStyles").Include(
                      "~/content/plugins/ionRangeSlider/ion.rangeSlider.css",
                      "~/content/plugins/ionRangeSlider/ion.rangeSlider.skinFlat.css"));

            // ionRange 
            bundles.Add(new ScriptBundle("~/scripts/plugins/ionRange").Include(
                      "~/scripts/plugins/ionRangeSlider/ion.rangeSlider.min.js"));

            // dataPicker styles
            bundles.Add(new StyleBundle("~/content/plugins/dataPickerStyles").Include(
                      "~/content/plugins/datapicker/datepicker3.css"));

            // dataPicker 
            bundles.Add(new ScriptBundle("~/scripts/plugins/dataPicker").Include(
                      "~/scripts/plugins/datapicker/bootstrap-datepicker.js"));

            // nouiSlider styles
            bundles.Add(new StyleBundle("~/content/plugins/nouiSliderStyles").Include(
                      "~/content/plugins/nouslider/jquery.nouislider.css"));

            // nouiSlider 
            bundles.Add(new ScriptBundle("~/scripts/plugins/nouiSlider").Include(
                      "~/scripts/plugins/nouslider/jquery.nouislider.min.js"));

            // jasnyBootstrap styles
            bundles.Add(new StyleBundle("~/content/plugins/jasnyBootstrapStyles").Include(
                      "~/content/plugins/jasny/jasny-bootstrap.min.css"));

            // jasnyBootstrap 
            bundles.Add(new ScriptBundle("~/scripts/plugins/jasnyBootstrap").Include(
                      "~/scripts/plugins/jasny/jasny-bootstrap.min.js"));

            // switchery styles
            bundles.Add(new StyleBundle("~/content/plugins/switcheryStyles").Include(
                      "~/content/plugins/switchery/switchery.css"));

            // switchery 
            bundles.Add(new ScriptBundle("~/scripts/plugins/switchery").Include(
                      "~/scripts/plugins/switchery/switchery.js"));

            // chosen styles
            bundles.Add(new StyleBundle("~/content/plugins/chosen/chosenStyles").Include(
                      "~/content/plugins/chosen/bootstrap-chosen.css"));

            // chosen 
            bundles.Add(new ScriptBundle("~/scripts/plugins/chosen").Include(
                      "~/scripts/plugins/chosen/chosen.jquery.js"));

            // knob 
            bundles.Add(new ScriptBundle("~/scripts/plugins/knob").Include(
                      "~/scripts/plugins/jsKnob/jquery.knob.js"));

            // wizardSteps styles
            bundles.Add(new StyleBundle("~/content/plugins/wizardStepsStyles").Include(
                      "~/content/plugins/steps/jquery.steps.css"));

            // wizardSteps 
            bundles.Add(new ScriptBundle("~/scripts/plugins/wizardSteps").Include(
                      "~/scripts/plugins/steps/jquery.steps.min.js"));

            // dropZone styles
            bundles.Add(new StyleBundle("~/content/plugins/dropzone/dropZoneStyles").Include(
                      "~/content/plugins/dropzone/basic.css",
                      "~/content/plugins/dropzone/dropzone.css"));

            // dropZone 
            bundles.Add(new ScriptBundle("~/scripts/plugins/dropZone").Include(
                      "~/scripts/plugins/dropzone/dropzone.js"));

            // summernote styles
            bundles.Add(new StyleBundle("~/content/plugins/summernoteStyles").Include(
                      "~/content/plugins/summernote/summernote.css",
                      "~/content/plugins/summernote/summernote-bs3.css"));

            // summernote 
            bundles.Add(new ScriptBundle("~/scripts/plugins/summernote").Include(
                      "~/scripts/plugins/summernote/summernote.min.js"));

            // toastr notification 
            bundles.Add(new ScriptBundle("~/scripts/plugins/toastr").Include(
                      "~/scripts/plugins/toastr/toastr.min.js"));

            // toastr notification styles
            bundles.Add(new StyleBundle("~/content/plugins/toastrStyles").Include(
                      "~/content/plugins/toastr/toastr.min.css"));

            // color picker
            bundles.Add(new ScriptBundle("~/scripts/plugins/colorpicker").Include(
                      "~/scripts/plugins/colorpicker/bootstrap-colorpicker.min.js"));

            // color picker styles
            bundles.Add(new StyleBundle("~/content/plugins/colorpicker/colorpickerStyles").Include(
                      "~/content/plugins/colorpicker/bootstrap-colorpicker.min.css"));

            // image cropper
            bundles.Add(new ScriptBundle("~/scripts/plugins/imagecropper").Include(
                      "~/scripts/plugins/cropper/cropper.min.js"));

            // image cropper styles
            bundles.Add(new StyleBundle("~/content/plugins/imagecropperStyles").Include(
                      "~/content/plugins/cropper/cropper.min.css"));

            // jsTree
            bundles.Add(new ScriptBundle("~/scripts/plugins/jsTree").Include(
                      "~/scripts/plugins/jsTree/jstree.min.js"));

            // jsTree styles
            bundles.Add(new StyleBundle("~/content/plugins/jsTree").Include(
                      "~/content/plugins/jsTree/style.css"));

            // Diff
            bundles.Add(new ScriptBundle("~/scripts/plugins/diff").Include(
                      "~/scripts/plugins/diff_match_patch/javascript/diff_match_patch.js",
                      "~/scripts/plugins/preetyTextDiff/jquery.pretty-text-diff.min.js"));

            // Idle timer
            bundles.Add(new ScriptBundle("~/scripts/plugins/idletimer").Include(
                      "~/scripts/plugins/idle-timer/idle-timer.min.js"));

            // Tinycon
            bundles.Add(new ScriptBundle("~/scripts/plugins/tinycon").Include(
                      "~/scripts/plugins/tinycon/tinycon.min.js"));

            // Chartist
            bundles.Add(new StyleBundle("~/content/plugins/chartistStyles").Include(
                      "~/content/plugins/chartist/chartist.min.css"));

            // jsTree styles
            bundles.Add(new ScriptBundle("~/scripts/plugins/chartist").Include(
                      "~/scripts/plugins/chartist/chartist.min.js"));

            // Awesome bootstrap checkbox
            bundles.Add(new StyleBundle("~/content/plugins/awesomeCheckboxStyles").Include(
                      "~/content/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css"));

            // Clockpicker styles
            bundles.Add(new StyleBundle("~/content/plugins/clockpickerStyles").Include(
                      "~/content/plugins/clockpicker/clockpicker.css"));

            // Clockpicker
            bundles.Add(new ScriptBundle("~/scripts/plugins/clockpicker").Include(
                      "~/scripts/plugins/clockpicker/clockpicker.js"));

            // Date range picker Styless
            bundles.Add(new StyleBundle("~/content/plugins/dateRangeStyles").Include(
                      "~/content/plugins/daterangepicker/daterangepicker-bs3.css"));

            // Date range picker
            bundles.Add(new ScriptBundle("~/scripts/plugins/dateRange").Include(
                      // Date range use moment.js same as full calendar plugin 
                      "~/scripts/plugins/fullcalendar/moment.min.js",
                      "~/scripts/plugins/daterangepicker/daterangepicker.js"));

            // Sweet alert Styless
            bundles.Add(new StyleBundle("~/content/plugins/sweetAlertStyles").Include(
                      "~/content/plugins/sweetalert/sweetalert.css"));

            // Sweet alert
            bundles.Add(new ScriptBundle("~/scripts/plugins/sweetAlert").Include(
                      "~/scripts/plugins/sweetalert/sweetalert.min.js"));

            // Footable Styless
            bundles.Add(new StyleBundle("~/content/plugins/footableStyles").Include(
                      "~/content/plugins/footable/footable.core.css", new CssRewriteUrlTransform()));

            // Footable alert
            bundles.Add(new ScriptBundle("~/scripts/plugins/footable").Include(
                      "~/scripts/plugins/footable/footable.all.min.js"));

            // Select2 Styless
            bundles.Add(new StyleBundle("~/content/plugins/select2Styles").Include(
                      "~/content/plugins/select2/select2.min.css"));

            // Select2
            bundles.Add(new ScriptBundle("~/scripts/plugins/select2").Include(
                      "~/scripts/plugins/select2/select2.full.min.js"));

            // Masonry
            bundles.Add(new ScriptBundle("~/scripts/plugins/masonry").Include(
                      "~/scripts/plugins/masonary/masonry.pkgd.min.js"));

            // Slick carousel Styless
            bundles.Add(new StyleBundle("~/content/plugins/slickStyles").Include(
                      "~/content/plugins/slick/slick.css", new CssRewriteUrlTransform()));

            // Slick carousel theme Styless
            bundles.Add(new StyleBundle("~/content/plugins/slickThemeStyles").Include(
                      "~/content/plugins/slick/slick-theme.css", new CssRewriteUrlTransform()));

            // Slick carousel
            bundles.Add(new ScriptBundle("~/scripts/plugins/slick").Include(
                      "~/scripts/plugins/slick/slick.min.js"));

            // Ladda buttons Styless
            bundles.Add(new StyleBundle("~/content/plugins/laddaStyles").Include(
                      "~/content/plugins/ladda/ladda-themeless.min.css"));

            // Ladda buttons
            bundles.Add(new ScriptBundle("~/scripts/plugins/ladda").Include(
                      "~/scripts/plugins/ladda/spin.min.js",
                      "~/scripts/plugins/ladda/ladda.min.js",
                      "~/scripts/plugins/ladda/ladda.jquery.min.js"));

            // Dotdotdot buttons
            bundles.Add(new ScriptBundle("~/scripts/plugins/truncate").Include(
                      "~/scripts/plugins/dotdotdot/jquery.dotdotdot.min.js"));

            // Touch Spin Styless
            bundles.Add(new StyleBundle("~/content/plugins/touchSpinStyles").Include(
                      "~/content/plugins/touchspin/jquery.bootstrap-touchspin.min.css"));

            // Touch Spin
            bundles.Add(new ScriptBundle("~/scripts/plugins/touchSpin").Include(
                      "~/scripts/plugins/touchspin/jquery.bootstrap-touchspin.min.js"));

            // Tour Styless
            bundles.Add(new StyleBundle("~/content/plugins/tourStyles").Include(
                      "~/content/plugins/bootstrapTour/bootstrap-tour.min.css"));

            // Tour Spin
            bundles.Add(new ScriptBundle("~/scripts/plugins/tour").Include(
                      "~/scripts/plugins/bootstrapTour/bootstrap-tour.min.js"));

            // i18next Spin
            bundles.Add(new ScriptBundle("~/scripts/plugins/i18next").Include(
                      "~/scripts/plugins/i18next/i18next.min.js"));

            // Clipboard Spin
            bundles.Add(new ScriptBundle("~/scripts/plugins/clipboard").Include(
                      "~/scripts/plugins/clipboard/clipboard.min.js"));

            // c3 Styless
            bundles.Add(new StyleBundle("~/content/plugins/c3Styles").Include(
                      "~/content/plugins/c3/c3.min.css"));

            // c3 Charts
            bundles.Add(new ScriptBundle("~/scripts/plugins/c3").Include(
                      "~/scripts/plugins/c3/c3.min.js"));

            // D3
            bundles.Add(new ScriptBundle("~/scripts/plugins/d3").Include(
                      "~/scripts/plugins/d3/d3.min.js"));

            // Markdown Styless
            bundles.Add(new StyleBundle("~/content/plugins/markdownStyles").Include(
                      "~/content/plugins/bootstrap-markdown/bootstrap-markdown.min.css"));

            // Markdown 
            bundles.Add(new ScriptBundle("~/scripts/plugins/markdown").Include(
                      "~/scripts/plugins/bootstrap-markdown/bootstrap-markdown.js",
                      "~/scripts/plugins/bootstrap-markdown/markdown.js"));

            // Datamaps
            bundles.Add(new ScriptBundle("~/scripts/plugins/datamaps").Include(
                      "~/scripts/plugins/topojson/topojson.js",
                      "~/scripts/plugins/datamaps/datamaps.all.min.js"));

            // Taginputs Styless
            bundles.Add(new StyleBundle("~/content/plugins/tagInputsStyles").Include(
                      "~/content/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css"));

            // Taginputs
            bundles.Add(new ScriptBundle("~/scripts/plugins/tagInputs").Include(
                      "~/scripts/plugins/bootstrap-tagsinput/bootstrap-tagsinput.js"));

            // Duallist Styless
            bundles.Add(new StyleBundle("~/content/plugins/duallistStyles").Include(
                      "~/content/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css"));

            // Duallist
            bundles.Add(new ScriptBundle("~/scripts/plugins/duallist").Include(
                      "~/scripts/plugins/dualListbox/jquery.bootstrap-duallistbox.js"));

            // SocialButtons styles
            bundles.Add(new StyleBundle("~/content/plugins/socialButtonsStyles").Include(
                      "~/content/plugins/bootstrapSocial/bootstrap-social.css"));

            // Typehead
            bundles.Add(new ScriptBundle("~/scripts/plugins/typehead").Include(
                      "~/scripts/plugins/typehead/bootstrap3-typeahead.min.js"));

            // Pdfjs
            bundles.Add(new ScriptBundle("~/scripts/plugins/pdfjs").Include(
                      "~/scripts/plugins/pdfjs/pdf.js"));

            // Touch Punch 
            bundles.Add(new StyleBundle("~/scripts/plugins/touchPunch").Include(
                        "~/scripts/plugins/touchpunch/jquery.ui.touch-punch.min.js"));

            // WOW 
            bundles.Add(new StyleBundle("~/scripts/plugins/wow").Include(
                        "~/scripts/plugins/wow/wow.min.js"));

            // Text spinners styles
            bundles.Add(new StyleBundle("~/content/plugins/textSpinnersStyles").Include(
                      "~/content/plugins/textSpinners/spinners.css"));

            // Password meter 
            bundles.Add(new StyleBundle("~/scripts/plugins/passwordMeter").Include(
                        "~/scripts/plugins/pwstrength/pwstrength-bootstrap.min.js",
                        "~/scripts/plugins/pwstrength/zxcvbn.js"));

            // input-mask
            bundles.Add(new StyleBundle("~/scripts/plugins/input-mask").Include("~/scripts/plugins/input-mask/inputmask.js",
                        "~/scripts/plugins/input-mask/jquery.inputmask.js",
                        "~/scripts/plugins/input-mask/jquery.inputmask.regex.extensions.js",
                        "~/scripts/plugins/input-mask/jquery.inputmask.numeric.extensions.js"
                        ));

            // token-input gallery css styles
            bundles.Add(new StyleBundle("~/content/plugins/token-input").Include(
                      "~/content/plugins/token-input/token-input-facebook.css"));

            // token-input gallery
            bundles.Add(new ScriptBundle("~/scripts/plugins/token-input").Include(
                      "~/scripts/plugins/token-input/jquery.tokeninput.js"));
        }
    }
}
