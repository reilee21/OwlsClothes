
function previewImages() {
    var preview = document.getElementById('imagePreview');
    preview.innerHTML = '';
    var files = document.getElementById('pro-img').files;
    if (files) {
        var filesArray = Array.from(files);
        filesArray.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });
        filesArray.forEach.call(files, function (file) {
            var reader = new FileReader();
            reader.onload = function (event) {
                var img = document.createElement('img');
                img.src = event.target.result;
                img.title = file.name;
                img.style.maxWidth = '150px';
                img.style.margin = '10px';
                img.style.padding = '8px';
                preview.appendChild(img);
            };
            reader.readAsDataURL(file);
        });
    }
    document.getElementById('old_img').remove();
}

function addVariant() {
    var variantsDiv = document.getElementById('variants');
    var index = variantsDiv.children.length;
    var colorOptions = colors.map(color => `<option value="${color.ColorId}">${color.ColorName}</option>`).join('');

    var newVariant = `
    <div class="variant">
                    <h4>Thuộc tính ${ index + 1 }</h4>
                    <div class="row">
                        <div class="form-group col-12 col-lg-4">
                            <label> SKU</label>
                            <input name="Varriants[${index}].Sku" class="form-control input-validation-error" aria-invalid="true"
                                 id="Varriants_${index}__Sku"
                                aria-describedby="Varriants_${index}__Sku-error"
                                data-val-required="Không được để trống" data-val="true" />
                            <span asp-validation-for="Varriants[${index}].Sku" data-valmsg-replace="true" data-valmsg-for="Varriants[${index}].Sku" class="text-danger"></span>
                        </div>
                        <div class="form-group col-12 col-lg-4">
                            <label> Size</label>
                            <input name="Varriants[${index}].Size" class="form-control" />
                            <span asp-validation-for="Varriants[${index}].Size" class="text-danger"></span>
                        </div>
                        <div class="form-group col-12 col-lg-4">
                            <label > Màu</label>
                            <select class="form-control" name="Varriants[${index}].ColorId" style="height:max-content">
                                <option value=""></option>
                                ${colorOptions}
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-12 col-lg-4">
                            <label > Giá </label>
                            <input name="Varriants[${index}].Cost" class="form-control" data-val="true" data-val-number="The field Cost must be a number." data-val-range="Giá phải lớn hơn 0" data-val-range-max="1.7976931348623157E+308" data-val-range-min="0" 
                            id="Varriants_${index}__Cost" aria-describedby="Varriants_${index}__Cost-error"/>
                            <span class="text-danger field-validation-valid"
                                    data-valmsg-for="Varriants[${index}].Cost" data-valmsg-replace="true"></span>
                        </div>

                        <div class="form-group col-12 col-lg-4">
                            <label > Giá bán</label>
                           
                            <input name="Varriants[${index}].SalePrice" class="form-control" data-val="true" data-val-number="The field SalePrice must be a number." data-val-range="Giá bán phải lớn hơn 0" data-val-range-max="1.7976931348623157E+308" data-val-range-min="0" 
                            id="Varriants_${index}__SalePrice" aria-describedby="Varriants_${index}__SalePrice-error"/>
                            <span class="text-danger field-validation-valid"
                                    data-valmsg-for="Varriants[${index}].SalePrice" data-valmsg-replace="true"></span>
                        </div>
                        <div class="form-group col-12 col-lg-4">
                            <label> Số lượng</label>

                            <input class="form-control valid" type="number" data-val="true" data-val-range="Số lượng phải từ 0 trở lên"
                            data-val-range-max="2147483647" data-val-range-min="0" id="Varriants_${index}__Quantity" 
                            name="Varriants[${index}].Quantity" value="1" aria-describedby="Varriants_${index}__Quantity-error" aria-invalid="false">
                            <span class="text-danger field-validation-valid" data-valmsg-for="Varriants[${index}].Quantity" data-valmsg-replace="true"></span>
                        </div>
                    </div>
                 <button type="button" class="btn btn-danger" onclick="removeVariant(this)">Xóa</button>
                </div>
    `;
    variantsDiv.insertAdjacentHTML('beforeend', newVariant);
}
