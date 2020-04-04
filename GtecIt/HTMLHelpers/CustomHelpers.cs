using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GtecIt.HTMLHelpers
{
    public static class CustomMvcHelpers
    {

        public static MvcHtmlString AutoCompleteFor<TModel, TResult>(this HtmlHelper<TModel> htmlHelper,
                                                                     Expression<Func<TModel, TResult>> propertyToSet,
                                                                     string hiddenId, int? hiddenValue,
                                                                     string url, string modo, bool obrigatorio, string nomefuncao)
        {
            var textBoxValue = ModelMetadata.FromLambdaExpression(propertyToSet, htmlHelper.ViewData).Model;

            var textboxId = ExpressionHelper.GetExpressionText(propertyToSet).Replace(".", "_");

            string componente;

            var required = obrigatorio ? "required" : string.Empty;

            if (modo.ToLower() == "r")// p = prefetch
            {
                componente = string.Format(
                @"
                  <input type='hidden' id='{0}' name='{0}' value='{1}' >
                  <input class='form-control' id='{2}' name='{2}' value='{3}' placeholder='Digite o codígo ou desc...' {5} type='text' autocomplete='off'>
                  <script type='text/javascript'> 
                        <!-- AutoCompleteFor -->
                        $(function () {{
                              var lista = new Bloodhound({{
                                    datumTokenizer: function (d) {{ return Bloodhound.tokenizers.whitespace(d.descricao); }},
                                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                                    remote: {{
                                        wildcard: '%QUERY',
                                        url: '{4}?query=%QUERY',
                                        transform: function (response) {{
                                            return $.map(response.results, function (model) {{
                                                return {{
                                                    
                                                    id: model.Id,
                                                    codigo: model.Codigo,
                                                    descricao: model.Descricao,
                                                }}
                                            }});
                                        }}
                                    }}
                                }});
                                $('#{2}').typeahead(
                                {{
                                    hint: false,
                                    highlight: true,
                                    minLength: 3
                                }},
                                {{
                                    name: 'lista',
                                    displayKey: 'descricao',
                                    source: lista,
                                     templates: {{
                                        empty: [
                                            '<div class=""tt-suggestion"">',
                                            'Nenhum registro encontrado.',
                                            '</div>'
                                        ].join('\n')
                                    }}
                                }}).on('typeahead:selected', function (object, datum) {{

                                    $('#{0}').val(datum.id.toString());
                                }});
                          }});
                  </script>
                 ", hiddenId, hiddenValue, textboxId, textBoxValue, url, required);
            }
            else 
            {
                componente = string.Format(
                    @"
                    <input type='hidden' id='{0}' name='{0}' value='{1}' >
                    <input class='form-control' id='{2}' name='{2}' value='{3}' placeholder='Digite o codígo ou desc...' {5} type='text' autocomplete='off'>
                    <script type='text/javascript'> 
                    <!-- AutoCompleteFor -->
                    function {6} () {{
                           var _lista = [];
                           var lista = new Bloodhound({{
                                    datumTokenizer: function (d) {{ return Bloodhound.tokenizers.whitespace(d.descricao); }},
                                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                                    limit: Number.MAX_VALUE,
                                    prefetch: {{
                                        url: '{4}',
                                        cache: false,
                                        filter: function (response) {{
                                            _lista = response.results;
                                            return $.map(response.results, function (model) {{
                                                return {{
                                                    
                                                    id: model.Id,
                                                    codigo: model.Codigo,
                                                    descricao: model.Descricao,
                                                }}
                                            }});
                                        }}
                                    }}
                                }});

                                lista.initialize();

                                $('#{2}').typeahead(
                                {{
                                    hint: false,
                                    highlight: true,
                                    minLength: 1
                                }},
                                {{
                                    name: 'lista',
                                    displayKey: 'descricao',
                                    source: lista.ttAdapter(),
                                    templates: {{
                                        empty: [
                                            '<div class=""tt-suggestion"">',
                                            'Nenhum registro encontrado.',
                                            '</div>'
                                        ].join('\n')
                                    }}
                                }}).on('typeahead:selected', function (object, datum) {{
                                    $('#{0}').val(datum.id.toString());
                                }}).bind('blur', function () {{
                                    var valor = $(this).val();
                                    var encontrado = _lista.some(function (el) {{
                                        return el.Descricao === valor;
                                    }});

                                    if (!encontrado) {{
                                        $('#{0}').val('');
                                        $(this).val('');
                                    }}
                                }});
                    }}
              </script>

              ", hiddenId, hiddenValue, textboxId, textBoxValue, url, required, nomefuncao);
            }
            return MvcHtmlString.Create(componente);
        }
    }
}