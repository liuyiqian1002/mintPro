"use strict";

exports.__esModule = true;
exports.default = void 0;
var _default = {
    name: 'اسم',
    tel: 'هاتف',
    save: 'يحفظ',
    confirm: 'يتأكد',
    cancel: 'يلغي',
    delete: 'حذف',
    complete: 'اكتمال',
    loading: 'جار التحميل',
    telEmpty: 'يرجى ملء الهاتف',
    nameEmpty: 'الرجاء كتابة الاسم',
    nameInvalid: 'اسم مشوه',
    confirmDelete: 'هل أنت متأكد أنك تريد حذف',
    telInvalid: 'رقم الهاتف غير صحيح',
    vanCalendar: {
        end: 'نهاية',
        start: 'بداية',
        title: 'تقويم',
        startEnd: 'بداية/نهاية',
        weekdays: ['يوم الأحد', 'يَومُ الإثنين', 'يوم الثلاثاء', 'يوم الأربعاء', 'يوم الخميس', 'يوم الجمعة', 'يوم السبت'],
        monthTitle: function monthTitle(year, month) {
            return year + "/" + month;
        },
        rangePrompt: function rangePrompt(maxRange) {
            return "اختر ما لا يزيد عن " + maxRange + " أيام";
        }
    },
    vanContactCard: {
        addText: 'أضف معلومات الاتصال'
    },
    vanContactList: {
        addText: 'Add new contact'
    },
    vanPagination: {
        prev: 'Previous',
        next: 'Next'
    },
    vanPullRefresh: {
        pulling: 'Pull to refresh...',
        loosing: 'Loose to refresh...'
    },
    vanSubmitBar: {
        label: 'Total：'
    },
    vanCoupon: {
        unlimited: 'Unlimited',
        discount: function discount(_discount) {
            return _discount * 10 + "% off";
        },
        condition: function condition(_condition) {
            return "At least " + _condition;
        }
    },
    vanCouponCell: {
        title: 'Coupon',
        tips: 'No coupons',
        count: function count(_count) {
            return "You have " + _count + " coupons";
        }
    },
    vanCouponList: {
        empty: 'No coupons',
        exchange: 'Exchange',
        close: 'Close',
        enable: 'Available',
        disabled: 'Unavailable',
        placeholder: 'Coupon code'
    },
    vanAddressEdit: {
        area: 'Area',
        postal: 'Postal',
        areaEmpty: 'Please select a receiving area',
        addressEmpty: 'Address can not be empty',
        postalEmpty: 'Wrong postal code',
        defaultAddress: 'Set as the default address',
        telPlaceholder: 'Phone',
        namePlaceholder: 'Name',
        areaPlaceholder: 'Area'
    },
    vanAddressEditDetail: {
        label: 'Address',
        placeholder: 'Address'
    },
    vanAddressList: {
        add: 'Add new address'
    }
};
exports.default = _default;