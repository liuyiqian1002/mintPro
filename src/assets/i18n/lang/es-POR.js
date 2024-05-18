"use strict";

exports.__esModule = true;
exports.default = void 0;
var _default = {
    name: 'Nome',
    tel: 'telefone',
    save: 'save',
    confirm: 'confirme',
    cancel: 'cancelar',
    delete: 'deletar',
    complete: 'levar a cabo',
    loading: 'Carregando...',
    telEmpty: 'Por favor preencha o telefone',
    nameEmpty: 'Por favor, preencha seu nome',
    nameInvalid: 'Por favor insira o nome correto',
    confirmDelete: 'Tem certeza que deseja deletar',
    telInvalid: 'Por favor insira um número de telefone válido',
    vanCalendar: {
        end: 'o fim',
        start: 'começar',
        title: 'Seleção de data',
        confirm: 'determinar',
        startEnd: 'começar/o fim',
        weekdays: ['domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        monthTitle: function monthTitle(year, month) {
            return year + "/" + month;
        },
        rangePrompt: function rangePrompt(maxRange) {
            return "Escolha não mais do que " + maxRange + " dias";
        }
    },
    vanContactCard: {
        addText: 'Adicionar contatos'
    },
    vanContactList: {
        addText: 'Novo contato'
    },
    vanPagination: {
        prev: 'Página anterior',
        next: 'Próxima página'
    },
    vanPullRefresh: {
        pulling: 'Puxe para baixo para atualizar...',
        loosing: 'Solte para atualizar...'
    },
    vanSubmitBar: {
        label: 'total'
    },
    vanCoupon: {
        unlimited: 'Sem limite de uso',
        discount: function discount(_discount) {
            return _discount + "\u6298";
        },
        condition: function condition(_condition) {
            return "\u6EE1" + _condition + "\u5143\u53EF\u7528";
        }
    },
    vanCouponCell: {
        title: '优惠券',
        tips: '暂无可用',
        count: function count(_count) {
            return _count + "\u5F20\u53EF\u7528";
        }
    },
    vanCouponList: {
        empty: '暂无优惠券',
        exchange: '兑换',
        close: '不使用优惠券',
        enable: '可用',
        disabled: '不可用',
        placeholder: '请输入优惠码'
    },
    vanAddressEdit: {
        area: '地区',
        postal: '邮政编码',
        areaEmpty: '请选择地区',
        addressEmpty: '请填写详细地址',
        postalEmpty: '邮政编码格式不正确',
        defaultAddress: '设为默认收货地址',
        telPlaceholder: '收货人手机号',
        namePlaceholder: '收货人姓名',
        areaPlaceholder: '选择省 / 市 / 区'
    },
    vanAddressEditDetail: {
        label: 'Endereço',
        placeholder: 'Número da rua, número do andar do quarto e outras informações'
    },
    vanAddressList: {
        add: 'Adicionar endereço'
    }
};
exports.default = _default;